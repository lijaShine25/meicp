using System;
using System.Collections.Generic;
using System.Text;

public interface IEmployees
{
    void usp_EmployeesInsert(Class_Employees objName);

    void usp_EmployeesUpdate(Class_Employees objName);

    void usp_EmployeesDelete(Class_Employees objName);

    IList<Class_Employees> usp_EmployeesSelect();
}
