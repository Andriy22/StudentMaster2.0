using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    [HttpGet("get-register-data/{subjectId}/{isExtended}")]
    public IActionResult GetRegisterData(int subjectId, bool isExtended)
    {
        var RegisterData = new List<dynamic>();

        Thread.Sleep(1000);

        RegisterData.Add(new
        {
            header = "Відвідування",
            items = new List<dynamic>
            {
                new
                {
                    title = "Відвідування",
                    value = "0/0",
                    name = Guid.NewGuid().ToString()
                }
            }
        });

        var random = new Random();


        for (var i = 0; i <= random.Next(6, 15); i++)
        {
            var obj = new
            {
                header = "ЛР " + i,
                items = new List<dynamic>
                {
                    new
                    {
                        title = "Загальна",
                        value = random.Next(3, 5),
                        name = Guid.NewGuid().ToString()
                    }
                }
            };

            if (isExtended)
            {
                obj.items.Add(new
                {
                    title = "Звіт",
                    value = random.Next(3, 5),
                    name = Guid.NewGuid().ToString(),
                    id = Guid.NewGuid().ToString()
                });
                if (random.Next(0, 3) == 2)
                    obj.items.Add(new
                    {
                        title = "Якість",
                        value = random.Next(3, 5),
                        name = Guid.NewGuid().ToString(),
                        id = Guid.NewGuid().ToString()
                    });
                if (random.Next(0, 3) == 2)
                    obj.items.Add(new
                    {
                        title = "Захист",
                        value = random.Next(3, 5),
                        name = Guid.NewGuid().ToString(),
                        id = Guid.NewGuid().ToString()
                    });
            }

            RegisterData.Add(obj);
        }


        return Ok(RegisterData);
    }
}