using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class ChangeMaterialInBuildPass : MaexPass<ChangeMaterialInBuildPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ChangeMaterialInBuild";
        public override string DisplayName => "Modular Avatar Extensions Change Material In Build";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var changeMaterialInBuilds =
                avatar.GetComponentsInChildren<ModularAvatarExtensionsChangeMaterialInBuild>(true).Where(c => c)
                    .ToArray();

            LogC($"Find {changeMaterialInBuilds.Length} change material in build inside \"{avatar.FullName()}\"");

            foreach (var changeMaterialInBuild in changeMaterialInBuilds)
                using (ErrorReport.WithContextObject(changeMaterialInBuild))
                    try
                    {
                        var renderer = changeMaterialInBuild.GetComponent<Renderer>();
                        var materialChangeDatas = changeMaterialInBuild.MaterialChangeDatas.Distinct().ToArray();

                        if (renderer == null)
                            throw new("Renderer is not assigned or found in the target component.");

                        var newMaterials = new Material[renderer.sharedMaterials.Length];

                        for (var i = 0; i < newMaterials.Length; i++)
                            newMaterials[i] = renderer.sharedMaterials[i];

                        foreach (var materialChangeData in materialChangeDatas)
                            if (!newMaterials.TrySetValue(materialChangeData.materialIndex, materialChangeData.material))
                                throw new ArgumentOutOfRangeException(nameof(materialChangeData.materialIndex),
                                    $"Material index {materialChangeData.materialIndex} is out of range.");

                        renderer.materials = newMaterials;
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }
        }
    }
}