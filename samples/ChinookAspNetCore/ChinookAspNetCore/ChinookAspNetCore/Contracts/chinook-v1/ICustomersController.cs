// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;


namespace ChinookAspNetCore.ChinookV1
{
    public interface ICustomersController
    {

        IActionResult Get();
        IActionResult Post([FromBody] Models.Customer customer);
        IActionResult GetById(string id);
        IActionResult Put([FromBody] Models.Customer customer,string id);
        IActionResult Delete(string id);
    }
}
