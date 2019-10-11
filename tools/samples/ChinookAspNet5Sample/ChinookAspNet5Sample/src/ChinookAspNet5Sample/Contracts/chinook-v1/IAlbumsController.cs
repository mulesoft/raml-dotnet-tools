// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;


namespace ChinookAspNet5Sample.ChinookV1
{
    public interface IAlbumsController
    {

        IActionResult Get();
        IActionResult Post([FromBody] Models.Album content);
        IActionResult GetById(string id);
        IActionResult Put([FromBody] Models.Album content,string id);
        IActionResult Delete(string id);
    }
}
