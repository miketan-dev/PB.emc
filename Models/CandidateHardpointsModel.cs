using System.Collections.Generic;
using PB.emc.utilities;
using YamlDotNet.Serialization;

namespace PB.emc.Models
{
    public class CandidateHardpointsModel
    {
        [YamlMember(Alias = "data")] public DataContent Data { get; set; } = new DataContent();
    }

    public class DataContent
    {
        [YamlMember(Alias = "candidateHardpoints")]
        public HashSet<string> CandidateHardpoints { get; set; } = new HashSet<string>
        {
            "external_arm_lower",
            "external_arm_upper",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_bottom_left_upper",
            "external_bottom_right_upper",
            "external_top_head",
            "external_top_pelvis"
        };

        public HashSet<string> CandidateHardpointsTargeted { get; set; } = new HashSet<string>
        {
            "external_arm_lower",
            "external_arm_upper",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_bottom_left_upper",
            "external_bottom_right_upper",
            "external_top_head",
            "external_top_pelvis"
        };
    }
}