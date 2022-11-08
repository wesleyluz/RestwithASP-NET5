﻿using System.Collections.Generic;
using RestWithASPNET.Model;

namespace RestWithASPNET.Services
{
    public interface IPersonService
   {
        Person Create(Person person);
        Person FindbyId(long id);
        List<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);




   }
}
