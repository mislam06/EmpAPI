using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmpDataAccess;

namespace EmpAPI.Controllers
{
    public class EmpController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get( string gender = "All")
        {
            using(EmpDBEntities entities = new EmpDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Value for Gender must be All, Male, or Female. " + gender + " is invalid");
                }
                //return entities.Employees.ToList();
            }
        }
        [HttpGet]
        public HttpResponseMessage LoadEmployeesId(int id)
        {
            using (EmpDBEntities entities = new EmpDBEntities())
            {

                var entity =  entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with the " + id.ToString() + " not found!");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            using(EmpDBEntities entities = new EmpDBEntities())
            {
                try { 
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var massage = Request.CreateResponse(HttpStatusCode.Created, employee);
                    massage.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return massage;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using(EmpDBEntities entities = new EmpDBEntities())
            {
                try
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Recored has been deleted!");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee id = " + id.ToString() + " not foudn for delete!");
                    }
                }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
                
            }
        }
        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            using(EmpDBEntities entities = new EmpDBEntities())
            {
                try
                {

                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
               
                    if(entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                       return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Id = " + id.ToString() + " not found for update!");
                    }
                }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse (HttpStatusCode.BadRequest, ex);
                }


            }
        }
    }
}
