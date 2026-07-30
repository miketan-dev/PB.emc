using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace PB.emc.Models
{
    /// <summary>
    /// Model class for defining the root of the candidate hardpoints configuration.
    /// </summary>
    public class CandidateHardpointsModel
    {
        [YamlMember(Alias = "data")] public DataContent Data { get; set; } = new();
    }

    /// <summary>
    /// Class containing the candidate hardpoints data ready to be written into the YAML file.
    /// </summary>
    public class DataContent
    {
        [YamlMember(Alias = "candidateHardpoints")]
        public HashSet<string> CandidateHardpoints { get; set; } =
        [
            "external_arm_lower",
            "external_arm_upper",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_bottom_left_upper",
            "external_bottom_right_upper",
            "external_top_head",
            "external_top_pelvis"
        ];

        [YamlMember(Alias = "candidateHardpointsTargeted")]
        public HashSet<string> CandidateHardpointsTargeted { get; set; } =
        [
            "external_arm_lower",
            "external_arm_upper",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_bottom_left_upper",
            "external_bottom_right_upper",
            "external_top_head",
            "external_top_pelvis"
        ];
    }
}