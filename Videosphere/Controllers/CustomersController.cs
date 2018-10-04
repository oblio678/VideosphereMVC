﻿using System;
using System.Collections.Generic;
using System.Data.Entity; //metoda Include()
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videosphere.Models;
using Videosphere.ViewModels;

namespace Videosphere.Controllers
{
    public class CustomersController : Controller
    {
        //pobieranie customersow z bazy a nie hard-coded:

        private ApplicationDbContext _context; //convention

        public CustomersController() //convention
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) //wzorzec dispose (convention).
        {
            _context.Dispose();
        }

        public ActionResult New() //Custromers/New
        {
            //do dropdown potrzebuje liste membershipTypes z bazy. (najpierw DbSet)

            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new NewCustomerViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View(viewModel); 
            //membershipTypes lepiej przez ViewModel bo pozniej potrzebuje przeslac customersow w razie implementacji edycji.
        }

        [HttpPost]
        public ActionResult Create(Customer customer) //model binding (MVC bind this model to the request data)
        {
            return View(); //zamiast NewCustomerViewModel moge Customer customer i MVC jest smart enough :) (Property sa z prefix Customer. )
        }

        // GET: Customers
        public ViewResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList(); //teraz z bazy. Include - Eager Loading.

            //entity framework nie robi zapytania sql. pobierze customersow podczas iteracji przez ten obiekt lub .ToList().

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id); //Include - Eager Loading.
            //tutaj query bedzie od razu przez SingleOrDefault(...).

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }
    }
}