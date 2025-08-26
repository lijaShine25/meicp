using System;
using System.Collections.Generic;
using System.Text;

public interface ICFTeams
{
    void usp_CFTeamsInsert(Class_CFTeams objName);

    void usp_CFTeamsUpdate(Class_CFTeams objName);

    void usp_CFTeamsDelete(Class_CFTeams objName);

    IList<Class_CFTeams> usp_CFTeamsSelect();
}
