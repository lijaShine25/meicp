using System;
using System.Collections.Generic;
using System.Text;

public interface Imachines
{
    void usp_machinesInsert(Class_machines objName);

    void usp_machinesUpdate(Class_machines objName);

    void usp_machinesDelete(Class_machines objName);

    IList<Class_machines> usp_machinesSelect();
}
