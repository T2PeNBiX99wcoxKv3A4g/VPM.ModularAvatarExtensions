using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [Serializable]
    [PublicAPI]
    public struct IconUsedPathReportData
    {
        public string iconName;
        public List<IconUsedPathData> usedPaths;

        public IconUsedPathReportData(string iconName, List<IconUsedPathData> usedPaths)
        {
            this.iconName = iconName;
            this.usedPaths = usedPaths;
        }
    }
}