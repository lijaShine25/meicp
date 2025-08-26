using System;
using System.Collections.Generic;
using System.Text;

public interface IEvaluationTech
{
    void usp_EvaluationTechInsert(Class_EvaluationTech objName);

    void usp_EvaluationTechUpdate(Class_EvaluationTech objName);

    void usp_EvaluationTechDelete(Class_EvaluationTech objName);

    IList<Class_EvaluationTech> usp_EvaluationTechSelect();
}
