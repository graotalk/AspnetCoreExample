﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreExample.Repository;
using AspNetCoreExample.SqlData.Northwind;
using AspNetCoreExample.SqlData.Vega;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreExample.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork<NORTHWNDContext> _northwindUnitOfWork;

        public ProductsController(IMapper mapper, IUnitOfWork<NORTHWNDContext> northwindUnitOfWork)
        {
            _mapper = mapper;
            _northwindUnitOfWork = northwindUnitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            try
            {
                var products = _northwindUnitOfWork.Repository<Products>().Where(x => x.CategoryId > 0).Include(y => y.Category);
                var mapped = _mapper.Map<IEnumerable<Products>, IEnumerable<Domain.Northwind.Product>>(products);
                return Ok(mapped);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}