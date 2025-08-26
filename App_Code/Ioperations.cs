using System;
using System.Collections.Generic;
using System.Text;

public interface Ioperations
{
    void usp_operationsInsert(Class_operations objName);

    void usp_operationsUpdate(Class_operations objName);

    void usp_operationsDelete(Class_operations objName);

    IList<Class_operations> usp_operationsSelect();
}
