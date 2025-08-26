using System;
using System.Collections.Generic;
using System.Text;

public interface Iparts
{
    IList<Class_parts> usp_partsInsert(Class_parts objName);

    void usp_partsUpdate(Class_parts objName);

    void usp_partsDelete(Class_parts objName);

    IList<Class_parts> usp_partsSelect();
    IList<Class_parts> usp_PartsMappingSelect();
    IList<Class_parts> usp_partsSelect_mapping();
}
