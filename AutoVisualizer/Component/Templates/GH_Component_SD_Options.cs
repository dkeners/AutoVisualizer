using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.Templates
{
    public abstract class GH_Component_SD_Options : GH_Component
    {
        public GH_Component_SD_Options(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        public T GetInputValue<T>(IGH_DataAccess DA, int index, T defaultValue)
        {
            T value = defaultValue;
            DA.GetData(index, ref value);
            return value;
        }

        //public T GetInputList<T>(IGH_DataAccess DA, int index, T defaultValue)
        //{
        //    T value = defaultValue;
        //    DA.GetDataList<T>(index, value);
        //    return value;
        //}
    }
}