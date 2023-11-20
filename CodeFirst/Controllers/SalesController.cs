using CodeFirst.Models;
using CodeFirst.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeFirst.Controllers
{
    public class SalesController : Controller
    {
        CodeDB db = new CodeDB();

        [HttpGet]
        public ActionResult Single(int? id)
        {
            var item = new List<SelectListItem>
            {
            new SelectListItem { Value = "Mobile", Text = "Mobile"},
            new SelectListItem { Value = "Laptop", Text = "Laptop" }
            };

            var oSale = db.SaleMasters
                .Where(x => x.SaleId == id)
                .Select(oSM => new VmSale
                {
                    SaleId = oSM.SaleId,
                    CreateDate = oSM.CreateDate,
                    CustomerAddress = oSM.CustomerAddress,
                    CustomerName = oSM.CustomerName,
                    Photo = oSM.Photo,
                    Gender = oSM.Gender,
                    ProductType = oSM.ProductType,
                    SaleDetails = oSM.SaleDetails
                        .Select(oSD => new VmSale.VmSaleDetail
                        {
                            SaleDetailId = oSD.SaleDetailId,
                            SaleId = oSD.SaleId,
                            Price = oSD.Price,
                            ProductName = oSD.ProductName,
                            Qty = oSD.Qty
                        }).ToList()
                })
                .FirstOrDefault();

            ViewData["List"] = db.SaleMasters.ToList();
            ViewData["L"] = db.SaleDetails.ToList();
            ViewData["item"] = item;
            return View(oSale ?? new VmSale());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Single(VmSale model, HttpPostedFileBase img, FormCollection form)
        {
            var oSaleMaster = db.SaleMasters.Include("SaleDetails").FirstOrDefault(x => x.SaleId == model.SaleId);
            string filename = img?.FileName;

            if (img != null)
            {
                string path = Path.Combine(Server.MapPath("~/Picture"), filename);
                img.SaveAs(path);
            }

            if (oSaleMaster == null)
            {
                oSaleMaster = new SaleMaster
                {
                    CreateDate = model.CreateDate,
                    CustomerAddress = model.CustomerAddress,
                    CustomerName = model.CustomerName,
                    Gender = model.Gender,
                    ProductType = form["SelectedValue"],
                    Photo = "/Picture/" + filename
                };
                db.SaleMasters.Add(oSaleMaster);
                ViewBag.Message = "inserted successfully.";

                var listSaleDetail = model.ProductName
                    .Where((name, i) => !string.IsNullOrEmpty(name))
                    .Select((name, i) => new SaleDetail
                    {
                        Price = model.Price[i],
                        ProductName = name,
                        Qty = model.Qty[i],
                        SaleId = oSaleMaster.SaleId
                    }).ToList();

                if (listSaleDetail.Any())
                {
                    db.SaleDetails.AddRange(listSaleDetail);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Product name or attachment required.";
                }
            }
            else
            {
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.Gender = model.Gender;
                oSaleMaster.Photo = (filename != null) ? "/Picture/" + filename : oSaleMaster.Photo;
                oSaleMaster.ProductType = form["SelectedValue"];
                ViewBag.Message = "updated successfully.";

                db.SaleDetails.RemoveRange(oSaleMaster.SaleDetails);

                var listSaleDetail = model.ProductName
                    .Where((name, i) => !string.IsNullOrEmpty(name))
                    .Select((name, i) => new SaleDetail
                    {
                        Price = model.Price[i],
                        ProductName = name,
                        Qty = model.Qty[i],
                        SaleId = oSaleMaster.SaleId
                    }).ToList();

                if (listSaleDetail.Any())
                {
                    db.SaleDetails.AddRange(listSaleDetail);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Product name or attachment required.";
                }
            }

            return RedirectToAction("Single");
        }

        [HttpGet]
        public ActionResult SingleDelete(int id)
        {
            var oSaleMaster = db.SaleMasters.Include("SaleDetails").FirstOrDefault(x => x.SaleId == id);

            if (oSaleMaster != null)
            {
                db.SaleDetails.RemoveRange(oSaleMaster.SaleDetails);
                db.SaleMasters.Remove(oSaleMaster);
                db.SaveChanges();
            }

            return RedirectToAction("Single");
        }
    }
}