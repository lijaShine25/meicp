using System;
using System.Collections.Generic;
using System.Text;

public interface ISampleFrequency
{
    void usp_SampleFrequencyInsert(Class_SampleFrequency objName);

    void usp_SampleFrequencyUpdate(Class_SampleFrequency objName);

    void usp_SampleFrequencyDelete(Class_SampleFrequency objName);

    IList<Class_SampleFrequency> usp_SampleFrequencySelect();
}
