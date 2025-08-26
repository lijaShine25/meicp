using System;
using System.Collections.Generic;
using System.Text;

public interface ICFTeamEmployees
{
    void usp_CFTeamEmployeesInsert(Class_CFTeamEmployees objName);

    void usp_CFTeamEmployeesUpdate(Class_CFTeamEmployees objName);

    void usp_CFTeamEmployeesDelete(Class_CFTeamEmployees objName);

    IList<Class_CFTeamEmployees> usp_CFTeamEmployeesSelect();
}
