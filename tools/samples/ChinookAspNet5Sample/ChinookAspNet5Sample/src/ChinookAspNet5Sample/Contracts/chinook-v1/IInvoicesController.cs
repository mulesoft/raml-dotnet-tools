// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;


namespace ChinookAspNet5Sample.ChinookV1
{
    public interface IInvoicesController
    {

        IActionResult Get();
        IActionResult Post([FromBody] Models.Invoice content);
        IActionResult GetById(string id);
        IActionResult Put([FromBody] Models.Invoice content,string id);
        IActionResult Delete(string id);
    }
}
