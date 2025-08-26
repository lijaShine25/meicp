using System;
using System.Collections.Generic;
using System.Text;

public interface IControlMethods
{
    void usp_ControlMethodsInsert(Class_ControlMethods objName);

    void usp_ControlMethodsUpdate(Class_ControlMethods objName);

    void usp_ControlMethodsDelete(Class_ControlMethods objName);

    IList<Class_ControlMethods> usp_ControlMethodsSelect();
}
