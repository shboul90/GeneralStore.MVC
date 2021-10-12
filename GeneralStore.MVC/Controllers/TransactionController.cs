using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            return View(_db.Transactions.ToList());
        }

        // GET: Transaction/{id}
        public ActionResult Details(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);

            if(transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transaction/Create
        // viewData / ViewBags
        public ActionResult Create()
        {
            var viewModel = new CreateTransactionViewModel();

            viewModel.Customers = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerID.ToString()
            });

            viewModel.Products = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            return View(viewModel);
        }

        // Post: Transaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTransactionViewModel viewModel)
        {

            viewModel.Customers = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerID.ToString()
            });

            viewModel.Products = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            Transaction transaction = new Transaction()
            {
                CustomerID = viewModel.CustomerID,
                ProductId = viewModel.ProductId
            };
            

            _db.Transactions.Add(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        // Get: Transaction/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        //POST: Transaction/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        // Get: Transaction/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            ViewData["Customers"] = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerID.ToString()
            });

            ViewData["Products"] = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            return View(transaction);
        }

        // POST: Transaction/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {

            _db.Entry(transaction).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}