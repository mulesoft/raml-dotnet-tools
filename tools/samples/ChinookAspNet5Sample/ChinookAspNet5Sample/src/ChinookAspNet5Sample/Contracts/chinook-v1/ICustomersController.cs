// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;


namespace ChinookAspNet5Sample.ChinookV1
{
    public interface ICustomersController
    {

        IActionResult Get();
        IActionResult Post([FromBody] Models.Customer content);
        IActionResult GetById(string id);
        IActionResult Put([FromBody] Models.Customer content,string id);
        IActionResult Delete(string id);
    }
}
