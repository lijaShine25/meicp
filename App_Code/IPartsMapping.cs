using System;
using System.Collections.Generic;
using System.Text;

public interface IPartsMapping
{
    void usp_PartsMappingInsert(Class_PartsMapping objName);

    void usp_PartsMappingUpdate(Class_PartsMapping objName);

    void usp_PartsMappingDelete(Class_PartsMapping objName);

    IList<Class_PartsMapping> usp_PartsMappingSelect();
    IList<Class_PartsMapping> usp_PartsMappingSelect_CPApproval();
}
