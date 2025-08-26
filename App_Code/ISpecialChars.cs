using System;
using System.Collections.Generic;
using System.Text;

public interface ISpecialChars
{
    void usp_SpecialCharsInsert(Class_SpecialChars objName);

    void usp_SpecialCharsUpdate(Class_SpecialChars objName);

    void usp_SpecialCharsDelete(Class_SpecialChars objName);

    IList<Class_SpecialChars> usp_SpecialCharsSelect();
}
