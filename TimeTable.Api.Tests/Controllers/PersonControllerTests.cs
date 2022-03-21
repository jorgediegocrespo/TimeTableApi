using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Api.Tests.Controllers.Base;
using TimeTable.Application.Constants;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Api.Tests.Controllers
{
    [TestClass]
    public class PersonControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/person";

        [TestMethod]
        public async Task Get_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_NullBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_PageSizeTooSortBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 0, PageNumber = 1 };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_PageSizeTooLongBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 101, PageNumber = 1 };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_NotFound()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/items/1456");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/ownItem");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/ownItem");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoGBxMTERYTExMWFhMWFhYWFhYRFhYQFhYWFhkYGBgYGRYaHysiGhwoHxgWJDQjKCwuMTExGSE3PDcwOyswMS4BCwsLDw4PHRERHDAoIikwMDAwMDAwMDAwMC4wMTAwMC4wMDAwMDAwMDAwLjAwMDAwMDAwMDAwMDAwMDAwMDAwMP/AABEIAOEA4QMBIgACEQEDEQH/xAAbAAEAAgMBAQAAAAAAAAAAAAAAAQYCBAUDB//EAEEQAAIBAgEIBwUHAgQHAAAAAAABAgMRBAUGEiExQVFxEyJhgZGxwTJSgqHRBzRCYnKS8LLhIyTC8RQzU2OTotL/xAAaAQEAAwEBAQAAAAAAAAAAAAAAAwQFAgEG/8QALxEAAgEDAwAHCAMBAAAAAAAAAAECAwQREiExQVFhcbHR8AUTMoGRocHhIjNC8f/aAAwDAQACEQMRAD8A+zGLYciUgCEjIgkAAAAAEAC5IIAJAAAAIAJMGyWyUgAkSQSAAAAACABckEAEgAAAEAEgi4ACRIAAIJAABDITuASSAAAcfKOX6dPqx68ux9Vc5fQ4GMytVqe1K0fdj1V/fvJ4W85b8IqVb2nT2W77PP8A6WnE5UpU/amr8I9Z+C2HOrZ0Q/BBv9TUfK5WwWY2sFzuUJ39V/DhffxOxPOeq9kYrucvU8ZZw13+K3KK9TmglVGmv8ogdzWf+n67jpRzhr+8vCJ7Qzmqraovua9TjgOjTf8AlBXNZf6f18yxUc6V+Om12pqXydjoYbLNGeyaT4S6r+eplNBHK1g+NieF/Vjzhn0IkomDyjVpexN29164+D9DvZPzjhPVUWg+O1P1RWnbzjxuXqV9Tns9n2+Z3CTCMk1da0+GszK5cAAAIJBDAJIBIBFgSAACCQAAQAASaeUcfCjHSlt3RW2T/m89SbeEeSkorL4M8XjIUo6U3ZfNvglvZVsq5anWvFdWn7q2v9T9DVx2MnVnpTfJLZFcEeBoUrdQ3e7MW4vJVP4x2X3YAJUSwU0iAJHHynnDGm3GmtOS2u/VT9T1bnjaR2AU6vlytL8eiuEEo/PaassZVe2pN/HL6nuk51ovYKJHGVFsqT7pyXqe9LLFeOyrJ/qtLzGka0XQFZw+c9Re3CMuV4P1XyOphMvUZ6m9B/n1L92wYZ6pJnSATBydG7k3KtSk9TvDfB7O7gy1ZPx8KsbwetbYvauf1KQemGxEqclKDtJfyz4ogq0Iz3WzLdvdypbPePh3eXgX8HOyRlSNaPCa9qPquw32zOlFxeGbUJqa1R4MgQiTw6IJAAAIABJAJAIJIFwDwxuJjTg5y2Lxb3JdpS8djJVZucu5borgjay9lHpalov/AA46o9r3yOcaNvR0LL5MS8uPeS0x+Ffd9YBKMrWLGSmkIq2tmDlcNmFWrGKvKSiuMnZDHSet9Byc5souEVTi7SktbW6P9/RlXNjKGKdWrKfF6uxLUl4GuSpYIJPLAAPTkE21EAHoNzJ3/Dt2raa7YtaPerXXzNMBoIueAwEIJOlOWi9dtJTg+5rysbxTckZVlRlvdN+1H1XB+Zb6NVTipRd4tXTRG0SxeTMEpGVrczlneD0w9aVOSnF2ktn07S35Lx8a0NJanskuD+hSWbWSce6FRS/C9UlxX1RBWo61lclq1ufdSw/hfrPn2F5B505qSTTumrpremehmm4AAAARcAEgAAHHzlx3R0tGL607rkt79O87BSMs4vpKspfhWqPJb+93feT28NU9+EVL2r7unhcvbz9dppgA0jDCZLZAABVM58W51tC/Vhqt+Z62/TuLWUXKEr1qj4zn/UzqPJxPg8AZU4OTUYq8m0kltbepI+srMTCyw9OnOFqkYJOrTejNy2yb3S132pnNWvGljPSSW9rOvnT0HyQFwy19nOIpXlQaqw4LqTS/S9T7n3HKq5o4tUo1VSlKLTuop6cGm01KDSkndPYmeqvTe6aPJWtaLacWcQHWoZs4mVPpOhkovVBNPTm3uhBK7W++pW3ndyP9m1edpV5xpR91f4k/l1V4vkJV6ceWIW1WbwospgPrtHMXCQozpxhec4Sj0tXrzTaspLdFp8Ej5LWpOEpQkrSi3GS4NOzXijylXjVzjoPbi1nQxq6eowO3mxj3CXRS9mfst7FLgufnbicVHZzcpRqKpSls6s4tbYyWrSi9z2eBI+CCPJZ0wRC9lfbvtxJIyYAAAseamOunSb1x1x9V3PzLAULBYl06kZr8LvzW9eFy8xmmk1rTSa5PeZ1zDTPK6fE2bGrrp6XyvD1sehARJXLwAAABBIBpZYxPR0Zy32suctS8ykllzsq9SEOMm38Or/UVqTNC1jiGesxr+WamOpAAFkogAAAouUY2rVF+ef8AUy9FPzjpaNeX5rS8Vr+aZ1Hk4nwdj7NcldNjFUkrxoLTf6tkF43l8J9bKx9nWSegwcZNder15fpfsL9tn8TLOZdxU11G+jg+gsqPuqKT5e7+ZFgSCAtAEEgA+R/aZk3osa5pdSsukXDSXVmvFJ/EfWmyqfabkvpcH0iV5UZafwPqzXlL4SxbT01F27FS+pe8ovrW/wBP0fK6VJyvZXsnJ8ltZ2sz4deo+EYrxbfoY5o0b1JyexR0f3P6RfidbIuA6GM1xm7fpWqPr4mo30Hz8V0nQABwSAAAAtubWI06CT2wbXdtXydu4qR3M0Ktqk4cYqX7Xb/UQXMc089Rbsp6ayXXt+fwWYkAzTcAIABJg2LmSQBVs7an+LGPCKfe2/ojjHSzmf8AmJdmj5X9TmmrRWKce4+euXmtLv8ADYAAkIQAAAc3ObIjVXCSnrjXkoO17qOnC13xam/A6R1ss0elweFqLbSxFC/LpFTfmn3EdSbhjHrbYnoUo1NSfKWV9Vn7FqjFJWWpLUkjMAyj6BgAhsAkwuTtJSACR5YmhGpCUJK8ZRcZLipKzPUkA+aZr5BnHCzq3Wqck09toNRbvz0jYLDKKo4Br3nN/wDkqSn5SK8alKo55b6zAuKUaTjFdSz6+QABKVwAAAdDN2pbEQ7dJf8Aq36I55t5FdsRT/UvnqOam8H3MkovFSL7V4l4AIbMg+jJBjpAAyAABTs5l/mJcl5HNOxnbG1ZPjFfJv8Ascc1aLzTj3Hz1ysVpLt/YABIQgAAA7Wb1eMoyozdk2nG/vJp2XbdRficUyoO0ovg15nFSGuOCSjUdOakj6ACSGZJ9GSQESAAAACCSGwCrZzYuLcKUHdQ22167WS7lfxOMAa8IKEdKPm6tV1ZObAAOjgAAAG1kZXxFP8AUjVN/N6N8RDs0n4Rf9jmbxFvsZJSWakV2rxLoRYIkyD6MAAAAgkAr2eFHVTnwbT79a8mV0ueXcPp0JpbUrr4dfldd5TDRtZZhjqMW/hpq560AAWCkAAAAwjJRtt/m0ZwMZL7CelFNb0n4noVvNnKUtLopa42bi/dtrtyLIZNSm4S0s+io1lVhqXpggkHBKAQSADwxU9GnJ8IyfgmezZWM5MpOUnRjqiraT4uyfhrJKdNzlhENeqqUNT+XecRAA1j50AA8PQCUibW5/7A9SMTtZo0r1JS91W75P8AszjNlqzWw+jR0ntnJvuWpeTfeQXMsU32lmyhqrLs39fM64JBmm6ARYAEkAkAixRsqYToq0obr3jyetfTuLycTOjBaUFVS60NvJ/R+bLFtPTPHWU76lrp5XK38ysAA0TEAJSMnZcwEshWXMwuAA2dLNn7xHk/JlrU9F2ezcyqZsfeI8peRbqsboz7r+z5eZs+z/6n3/hGYPCFSzsz2KxeJMSTWrVb6ls8wBWq31LZ5lSy594n8P8ASi0FXy394n8P9KLVp8b7vIo+0v6l3/hmmAC+YwJSEVrMm0tS/n8sMnqXSxdLmYAA8bM6FFznGEdsmkvqXujSUYqK2JJLkjgZqYK7dZ7urDn+J+niWQz7qeZaV0GxYUtMNb6fD1uAAVi+AQACQAADGUU1Z7DIAFJyxk90qjX4Hrg+zhzRpF3yngY1qbg9u2L4PiUzFYeVObhJWkv5ddhpUKutYfJh3dv7qWV8L48vL9HmACcqAA86teMfakl36/A9B1s2fvEeUvIuBQ83Mp01Xi9eirpytZLSVkXxmfdp613Gx7Pa9012/hGNSCe08JQkuNuw2OZkVS+aTk3vIZtypp7jzeHXEHuTXKvlz7xP4f6UW54e28o+W8ow6eT16LtoytdOyS57i1aL+b7jP9pNe7S7fwzEHnSrwl7Mk+/X4HoXzHAAAB74DByq1FCO/a+C3s8acHJqMVdt2SW9lxyNk1UYa9c5a5P0XYiKtV92tuegsW1u60t+Fz5G3QoqEVGKskrI9STF9hlm+SCF8zIAAAAAgXAJIuQ2SkADn5XyXGvHhNezL0fYdEHsZOLyjmcIzWmXB83yhXVGbp1E1NbVbwd9jXaaFXKz/DFLnrPomW8i08TDRmrSXszXtR+q7D55lvIVbDStNXg31Zx9l/8Ay+x/M1KFaNTZ8mHc2s6Tyt49fV3+fBqVcXOW2T5LUvkeUItuy1tkG/khRu22tLYk/PmWXsVDfwtBQiorvfFlqzayppLopvrL2G96W7mvLkVsmE2mmnZp3TW5or1aaqRwyehVdKepfM+hEHPyNlFVoX2TjqkvVdjOiZUouLwz6CE1OKlHhgEHNy3lLooWj7ctUV6iMXJ4QnNQjqlwaGcuVNtKD/W15fUq+Nw+nFx37nwZtN73rb1323v/ADaYN3NWlBQjhHz9eq6stUv+Fbas7Paj1p4ucdkn36/M2MrKGleLWl+JLX3mkWFuQG/SytJe1FPlqNzC46M5KEVLTk7KNrtvssc/JWS6uInoUo396T1Rj2yfptPoWb2btPDRv7dVrrTa18orcivXrQprHT1eZZt7adZ54XX5GeRMjKktKWuo+9RXBdvadcEGXKTk8s3adONOOmPAJAOTsEEgAAiwAJMNotczAIRJAuASAAAedWlGUXGSTi1ZqSumu1M9CACoZYzHjK88PLRfuTu4/DLbHk79xUMoZOq0Ho1acocG1eL5SWpn1886tKMk1JJp7VJJp80y1Su5x2lv4lGtYU57x/i/t9PI+SYfHzhvuuEtfgzpYfKMJbeq+3Z4ltx2Z2Gqa1GVOX/bdl+13XhY4eKzBqL/AJdaEuyadN/K5bjc0pcvBQnY1o8LPd+8DAYyVOopx71ukt6LrhcRGpFTi7prw4p9p8/WbeOpezDSXBTg14NpnazcrYilPRqUJxhLbZaUU/e1bO0huIQmtUWsrt5JrSdSlLROLw+x7MsuNxUaUHOWxbuL3JFMxmLc5ucn1n4JLYuSNvOCviKk7U6FRxjqjeLhHtevacV5uY6q+tTaX5pRivC9xQhGC1Saz3i7qVKktMYvC7Hz1+R54jKUI7Os+zZ4nOxGOnPfZcI6v9ywYXMKs/bqwgvy3m/RHawOZGHhrnpVH+Z6Mf2xt82yaVzSj05IIWVeXRjv9Z+xQ8HhalWWjShKcuEVe3PguZa8j5it2liJWX/Tg9fxT3cl4lww9CEI6MIxjFbopJeCPYq1Lycto7eJepez4R3m8v7fv5/Q18JhoU4KEIqMVsSVl/O02AQVDQBIIAJAAAAIuASDHWSASAAAAACCSGRFAEkgAAAAAgkgAkgIkAAAAAAAgkhhAAkAAAAAgEmDYBLkSkEiQAAAAAAAAACEGAASAAAAAAREAASJAAAAAAAAIiGAASAAAAADGWwRAAJZIAAAAB//2Q=="));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent("Test"), "Name");
            content.Add(new StringContent("test@test.com"), "Email");
            content.Add(new StringContent("1234_Test"), "Password");
            using HttpResponseMessage response = await client.PostAsync(url, content);


            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_NullBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_NameBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoGBxMTERYTExMWFhMWFhYWFhYRFhYQFhYWFhkYGBgYGRYaHysiGhwoHxgWJDQjKCwuMTExGSE3PDcwOyswMS4BCwsLDw4PHRERHDAoIikwMDAwMDAwMDAwMC4wMTAwMC4wMDAwMDAwMDAwLjAwMDAwMDAwMDAwMDAwMDAwMDAwMP/AABEIAOEA4QMBIgACEQEDEQH/xAAbAAEAAgMBAQAAAAAAAAAAAAAAAQYCBAUDB//EAEEQAAIBAgEIBwUHAgQHAAAAAAABAgMRBAUGEiExQVFxEyJhgZGxwTJSgqHRBzRCYnKS8LLhIyTC8RQzU2OTotL/xAAaAQEAAwEBAQAAAAAAAAAAAAAAAwQFAgEG/8QALxEAAgEDAwAHCAMBAAAAAAAAAAECAwQREiExQVFhcbHR8AUTMoGRocHhIjNC8f/aAAwDAQACEQMRAD8A+zGLYciUgCEjIgkAAAAAEAC5IIAJAAAAIAJMGyWyUgAkSQSAAAAACABckEAEgAAAEAEgi4ACRIAAIJAABDITuASSAAAcfKOX6dPqx68ux9Vc5fQ4GMytVqe1K0fdj1V/fvJ4W85b8IqVb2nT2W77PP8A6WnE5UpU/amr8I9Z+C2HOrZ0Q/BBv9TUfK5WwWY2sFzuUJ39V/DhffxOxPOeq9kYrucvU8ZZw13+K3KK9TmglVGmv8ogdzWf+n67jpRzhr+8vCJ7Qzmqraovua9TjgOjTf8AlBXNZf6f18yxUc6V+Om12pqXydjoYbLNGeyaT4S6r+eplNBHK1g+NieF/Vjzhn0IkomDyjVpexN29164+D9DvZPzjhPVUWg+O1P1RWnbzjxuXqV9Tns9n2+Z3CTCMk1da0+GszK5cAAAIJBDAJIBIBFgSAACCQAAQAASaeUcfCjHSlt3RW2T/m89SbeEeSkorL4M8XjIUo6U3ZfNvglvZVsq5anWvFdWn7q2v9T9DVx2MnVnpTfJLZFcEeBoUrdQ3e7MW4vJVP4x2X3YAJUSwU0iAJHHynnDGm3GmtOS2u/VT9T1bnjaR2AU6vlytL8eiuEEo/PaassZVe2pN/HL6nuk51ovYKJHGVFsqT7pyXqe9LLFeOyrJ/qtLzGka0XQFZw+c9Re3CMuV4P1XyOphMvUZ6m9B/n1L92wYZ6pJnSATBydG7k3KtSk9TvDfB7O7gy1ZPx8KsbwetbYvauf1KQemGxEqclKDtJfyz4ogq0Iz3WzLdvdypbPePh3eXgX8HOyRlSNaPCa9qPquw32zOlFxeGbUJqa1R4MgQiTw6IJAAAIABJAJAIJIFwDwxuJjTg5y2Lxb3JdpS8djJVZucu5borgjay9lHpalov/AA46o9r3yOcaNvR0LL5MS8uPeS0x+Ffd9YBKMrWLGSmkIq2tmDlcNmFWrGKvKSiuMnZDHSet9Byc5souEVTi7SktbW6P9/RlXNjKGKdWrKfF6uxLUl4GuSpYIJPLAAPTkE21EAHoNzJ3/Dt2raa7YtaPerXXzNMBoIueAwEIJOlOWi9dtJTg+5rysbxTckZVlRlvdN+1H1XB+Zb6NVTipRd4tXTRG0SxeTMEpGVrczlneD0w9aVOSnF2ktn07S35Lx8a0NJanskuD+hSWbWSce6FRS/C9UlxX1RBWo61lclq1ufdSw/hfrPn2F5B505qSTTumrpremehmm4AAAARcAEgAAHHzlx3R0tGL607rkt79O87BSMs4vpKspfhWqPJb+93feT28NU9+EVL2r7unhcvbz9dppgA0jDCZLZAABVM58W51tC/Vhqt+Z62/TuLWUXKEr1qj4zn/UzqPJxPg8AZU4OTUYq8m0kltbepI+srMTCyw9OnOFqkYJOrTejNy2yb3S132pnNWvGljPSSW9rOvnT0HyQFwy19nOIpXlQaqw4LqTS/S9T7n3HKq5o4tUo1VSlKLTuop6cGm01KDSkndPYmeqvTe6aPJWtaLacWcQHWoZs4mVPpOhkovVBNPTm3uhBK7W++pW3ndyP9m1edpV5xpR91f4k/l1V4vkJV6ceWIW1WbwospgPrtHMXCQozpxhec4Sj0tXrzTaspLdFp8Ej5LWpOEpQkrSi3GS4NOzXijylXjVzjoPbi1nQxq6eowO3mxj3CXRS9mfst7FLgufnbicVHZzcpRqKpSls6s4tbYyWrSi9z2eBI+CCPJZ0wRC9lfbvtxJIyYAAAseamOunSb1x1x9V3PzLAULBYl06kZr8LvzW9eFy8xmmk1rTSa5PeZ1zDTPK6fE2bGrrp6XyvD1sehARJXLwAAABBIBpZYxPR0Zy32suctS8ykllzsq9SEOMm38Or/UVqTNC1jiGesxr+WamOpAAFkogAAAouUY2rVF+ef8AUy9FPzjpaNeX5rS8Vr+aZ1Hk4nwdj7NcldNjFUkrxoLTf6tkF43l8J9bKx9nWSegwcZNder15fpfsL9tn8TLOZdxU11G+jg+gsqPuqKT5e7+ZFgSCAtAEEgA+R/aZk3osa5pdSsukXDSXVmvFJ/EfWmyqfabkvpcH0iV5UZafwPqzXlL4SxbT01F27FS+pe8ovrW/wBP0fK6VJyvZXsnJ8ltZ2sz4deo+EYrxbfoY5o0b1JyexR0f3P6RfidbIuA6GM1xm7fpWqPr4mo30Hz8V0nQABwSAAAAtubWI06CT2wbXdtXydu4qR3M0Ktqk4cYqX7Xb/UQXMc089Rbsp6ayXXt+fwWYkAzTcAIABJg2LmSQBVs7an+LGPCKfe2/ojjHSzmf8AmJdmj5X9TmmrRWKce4+euXmtLv8ADYAAkIQAAAc3ObIjVXCSnrjXkoO17qOnC13xam/A6R1ss0elweFqLbSxFC/LpFTfmn3EdSbhjHrbYnoUo1NSfKWV9Vn7FqjFJWWpLUkjMAyj6BgAhsAkwuTtJSACR5YmhGpCUJK8ZRcZLipKzPUkA+aZr5BnHCzq3Wqck09toNRbvz0jYLDKKo4Br3nN/wDkqSn5SK8alKo55b6zAuKUaTjFdSz6+QABKVwAAAdDN2pbEQ7dJf8Aq36I55t5FdsRT/UvnqOam8H3MkovFSL7V4l4AIbMg+jJBjpAAyAABTs5l/mJcl5HNOxnbG1ZPjFfJv8Ascc1aLzTj3Hz1ysVpLt/YABIQgAAA7Wb1eMoyozdk2nG/vJp2XbdRficUyoO0ovg15nFSGuOCSjUdOakj6ACSGZJ9GSQESAAAACCSGwCrZzYuLcKUHdQ22167WS7lfxOMAa8IKEdKPm6tV1ZObAAOjgAAAG1kZXxFP8AUjVN/N6N8RDs0n4Rf9jmbxFvsZJSWakV2rxLoRYIkyD6MAAAAgkAr2eFHVTnwbT79a8mV0ueXcPp0JpbUrr4dfldd5TDRtZZhjqMW/hpq560AAWCkAAAAwjJRtt/m0ZwMZL7CelFNb0n4noVvNnKUtLopa42bi/dtrtyLIZNSm4S0s+io1lVhqXpggkHBKAQSADwxU9GnJ8IyfgmezZWM5MpOUnRjqiraT4uyfhrJKdNzlhENeqqUNT+XecRAA1j50AA8PQCUibW5/7A9SMTtZo0r1JS91W75P8AszjNlqzWw+jR0ntnJvuWpeTfeQXMsU32lmyhqrLs39fM64JBmm6ARYAEkAkAixRsqYToq0obr3jyetfTuLycTOjBaUFVS60NvJ/R+bLFtPTPHWU76lrp5XK38ysAA0TEAJSMnZcwEshWXMwuAA2dLNn7xHk/JlrU9F2ezcyqZsfeI8peRbqsboz7r+z5eZs+z/6n3/hGYPCFSzsz2KxeJMSTWrVb6ls8wBWq31LZ5lSy594n8P8ASi0FXy394n8P9KLVp8b7vIo+0v6l3/hmmAC+YwJSEVrMm0tS/n8sMnqXSxdLmYAA8bM6FFznGEdsmkvqXujSUYqK2JJLkjgZqYK7dZ7urDn+J+niWQz7qeZaV0GxYUtMNb6fD1uAAVi+AQACQAADGUU1Z7DIAFJyxk90qjX4Hrg+zhzRpF3yngY1qbg9u2L4PiUzFYeVObhJWkv5ddhpUKutYfJh3dv7qWV8L48vL9HmACcqAA86teMfakl36/A9B1s2fvEeUvIuBQ83Mp01Xi9eirpytZLSVkXxmfdp613Gx7Pa9012/hGNSCe08JQkuNuw2OZkVS+aTk3vIZtypp7jzeHXEHuTXKvlz7xP4f6UW54e28o+W8ow6eT16LtoytdOyS57i1aL+b7jP9pNe7S7fwzEHnSrwl7Mk+/X4HoXzHAAAB74DByq1FCO/a+C3s8acHJqMVdt2SW9lxyNk1UYa9c5a5P0XYiKtV92tuegsW1u60t+Fz5G3QoqEVGKskrI9STF9hlm+SCF8zIAAAAAgXAJIuQ2SkADn5XyXGvHhNezL0fYdEHsZOLyjmcIzWmXB83yhXVGbp1E1NbVbwd9jXaaFXKz/DFLnrPomW8i08TDRmrSXszXtR+q7D55lvIVbDStNXg31Zx9l/8Ay+x/M1KFaNTZ8mHc2s6Tyt49fV3+fBqVcXOW2T5LUvkeUItuy1tkG/khRu22tLYk/PmWXsVDfwtBQiorvfFlqzayppLopvrL2G96W7mvLkVsmE2mmnZp3TW5or1aaqRwyehVdKepfM+hEHPyNlFVoX2TjqkvVdjOiZUouLwz6CE1OKlHhgEHNy3lLooWj7ctUV6iMXJ4QnNQjqlwaGcuVNtKD/W15fUq+Nw+nFx37nwZtN73rb1323v/ADaYN3NWlBQjhHz9eq6stUv+Fbas7Paj1p4ucdkn36/M2MrKGleLWl+JLX3mkWFuQG/SytJe1FPlqNzC46M5KEVLTk7KNrtvssc/JWS6uInoUo396T1Rj2yfptPoWb2btPDRv7dVrrTa18orcivXrQprHT1eZZt7adZ54XX5GeRMjKktKWuo+9RXBdvadcEGXKTk8s3adONOOmPAJAOTsEEgAAiwAJMNotczAIRJAuASAAAedWlGUXGSTi1ZqSumu1M9CACoZYzHjK88PLRfuTu4/DLbHk79xUMoZOq0Ho1acocG1eL5SWpn1886tKMk1JJp7VJJp80y1Su5x2lv4lGtYU57x/i/t9PI+SYfHzhvuuEtfgzpYfKMJbeq+3Z4ltx2Z2Gqa1GVOX/bdl+13XhY4eKzBqL/AJdaEuyadN/K5bjc0pcvBQnY1o8LPd+8DAYyVOopx71ukt6LrhcRGpFTi7prw4p9p8/WbeOpezDSXBTg14NpnazcrYilPRqUJxhLbZaUU/e1bO0huIQmtUWsrt5JrSdSlLROLw+x7MsuNxUaUHOWxbuL3JFMxmLc5ucn1n4JLYuSNvOCviKk7U6FRxjqjeLhHtevacV5uY6q+tTaX5pRivC9xQhGC1Saz3i7qVKktMYvC7Hz1+R54jKUI7Os+zZ4nOxGOnPfZcI6v9ywYXMKs/bqwgvy3m/RHawOZGHhrnpVH+Z6Mf2xt82yaVzSj05IIWVeXRjv9Z+xQ8HhalWWjShKcuEVe3PguZa8j5it2liJWX/Tg9fxT3cl4lww9CEI6MIxjFbopJeCPYq1Lycto7eJepez4R3m8v7fv5/Q18JhoU4KEIqMVsSVl/O02AQVDQBIIAJAAAAIuASDHWSASAAAAACCSGRFAEkgAAAAAgkgAkgIkAAAAAAAgkhhAAkAAAAAgEmDYBLkSkEiQAAAAAAAAACEGAASAAAAAAREAASJAAAAAAAAIiGAASAAAAADGWwRAAJZIAAAAB//2Q=="));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent("abc"), "Name");
            content.Add(new StringContent("test@test.com"), "Email");
            content.Add(new StringContent("1234_Test"), "Password");
            using HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_EmailBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent("Test"), "Name");
            content.Add(new StringContent("test"), "Email");
            content.Add(new StringContent("1234_Test"), "Password");
            using HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_PasswordBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent("Test"), "Name");
            content.Add(new StringContent("test@test.com"), "Email");
            content.Add(new StringContent(String.Empty), "Password");
            using HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent("Test"), "Name");
            content.Add(new StringContent("test@test.com"), "Email");
            content.Add(new StringContent("1234_Test"), "Password");

            using HttpResponseMessage response = await client.PostAsync(url, content);
            dynamic result = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(result.id > 0);
        }

        [TestMethod]
        public async Task Put_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent(PeopleInfo.EmployeeId.ToString()), "Id");
            content.Add(new StringContent("Employee updated"), "Name");
            using HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NullBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NameBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent(PeopleInfo.EmployeeId.ToString()), "Id");
            content.Add(new StringContent("123"), "Name");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Forbiden()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
                        
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent(PeopleInfo.EmployeeId.ToString()), "Id");
            content.Add(new StringContent("Test-Updated"), "Name");
            content.Add(new StringContent(Convert.ToBase64String(PeopleInfo.EmployeeRowVersion)), "RowVersion");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();
            
            using MultipartFormDataContent content = new MultipartFormDataContent();
            using ByteArrayContent baContent = new ByteArrayContent(Convert.FromBase64String("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"));
            content.Add(baContent, "Picture", "picture.jpg");
            content.Add(new StringContent(PeopleInfo.EmployeeId.ToString()), "Id");
            content.Add(new StringContent("Employee updated"), "Name");
            content.Add(new StringContent(Convert.ToBase64String(PeopleInfo.EmployeeRowVersion)), "RowVersion");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);
            DeleteRequest deleteRequest = new DeleteRequest { Id = PeopleInfo.AdminId, RowVersion = rndRowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);
            
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Conflict()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);
            DeleteRequest deleteRequest = new DeleteRequest { Id = PeopleInfo.AdminId, RowVersion = rndRowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);

            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.AreEqual(ErrorCodes.PERSON_DEFAULT, result.error_code);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();
            using TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var person = await AddTestPerson(factory, timeTableContext);

            DeleteRequest deleteRequest = new DeleteRequest { Id = person.Id, RowVersion = person.RowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOwn_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            using HttpClient client = factory.CreateClient();

            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);
            DeleteRequest deleteRequest = new DeleteRequest { Id = PeopleInfo.EmployeeId, RowVersion = rndRowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/deleteOwn", content);
            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOwn_Conflict()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);
            DeleteRequest deleteRequest = new DeleteRequest { Id = PeopleInfo.AdminId, RowVersion = rndRowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/deleteOwn", content);
            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.AreEqual(ErrorCodes.PERSON_DEFAULT, result.error_code);
        }

        [TestMethod]
        public async Task DeleteOwn_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            DeleteRequest deleteRequest = new DeleteRequest { Id = PeopleInfo.EmployeeId, RowVersion = PeopleInfo.EmployeeRowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"{url}/deleteOwn", content);
            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task<PersonEntity> AddTestPerson(WebApplicationFactory<Startup> factory, TimeTableDbContext context)
        {
            using var scope = factory.Services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            using UserManager<IdentityUser> userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            IdentityUser user = new IdentityUser
            {
                UserName = "Test",
                Email = "test@test.com",
            };

            await userManager.CreateAsync(user, Constants.AdminPassword);
            await userManager.AddToRoleAsync(user, RolesConsts.EMPLOYEE);

            string userId = await userManager.GetUserIdAsync(user);
            var person = new PersonEntity() { Name = "Test", UserId = userId, IsDefault = false };
            await context.People.AddAsync(person);

            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            return person;
        }
    }
}
