using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.POS
{
    public static class Converter
    {
        static readonly Dictionary<string, BasePOSType> typeMap = new Dictionary<string, BasePOSType>(StringComparer.OrdinalIgnoreCase);

        static Converter()
        {
            typeMap["AJ0"] = POSTags.Instance.JJ;
            typeMap["NN2"] = POSTags.Instance.NNS;
            typeMap["AV0"] = POSTags.Instance.RB;
            typeMap["NP0"] = POSTags.Instance.NNP;
            typeMap["AVP"] = POSTags.Instance.RP;
            typeMap["PNI"] = POSTags.Instance.PRP;
            typeMap["AVQ"] = POSTags.Instance.RP;
            typeMap["PRP"] = POSTags.Instance.PRP;
            typeMap["CJS"] = POSTags.Instance.IN;
            typeMap["VVB"] = POSTags.Instance.VB;
            typeMap["CJT"] = POSTags.Instance.IN;
            typeMap["VVD"] = POSTags.Instance.VBD;
            typeMap["CRD"] = POSTags.Instance.CD;
            typeMap["VVG"] = POSTags.Instance.VBG;
            typeMap["DT0"] = POSTags.Instance.DT;
            typeMap["VVN"] = POSTags.Instance.VBN;
            typeMap["NN1"] = POSTags.Instance.NN;
            typeMap["NN0"] = POSTags.Instance.NNS;
            typeMap["VVZ"] = POSTags.Instance.VB;
            typeMap["UNC"] = POSTags.Instance.UnknownWord;
            typeMap["at0"] = POSTags.Instance.DT;
            typeMap["prf"] = POSTags.Instance.IN;
            typeMap["cjc"] = POSTags.Instance.CC;
            typeMap["to0"] = POSTags.Instance.TO;
            typeMap["pnp"] = POSTags.Instance.PRP;
            typeMap["VBZ"] = POSTags.Instance.VBZ;
            typeMap["VBD"] = POSTags.Instance.VBD;
            typeMap["pos"] = POSTags.Instance.UnknownWord;
            typeMap["vbi"] = POSTags.Instance.VB;
            typeMap["vhb"] = POSTags.Instance.VBP;
            typeMap["vbb"] = POSTags.Instance.VBP;
            typeMap["xx0"] = POSTags.Instance.RB;
            typeMap["vhd"] = POSTags.Instance.VBD;
            typeMap["dps"] = POSTags.Instance.PRPS;
            typeMap["dtq"] = POSTags.Instance.WDT;
            typeMap["ex0"] = POSTags.Instance.EX;
            typeMap["vdb"] = POSTags.Instance.VBP;
            typeMap["VBN"] = POSTags.Instance.VBN;
            typeMap["vhz"] = POSTags.Instance.VBZ;
            typeMap["vm0"] = POSTags.Instance.MD;
            typeMap["pnq"] = POSTags.Instance.WP;
            typeMap["vdd"] = POSTags.Instance.VBD;
            typeMap["vvi"] = POSTags.Instance.VBP;
            typeMap["ord"] = POSTags.Instance.RB;
            typeMap["VBG"] = POSTags.Instance.VBG;
            typeMap["itj"] = POSTags.Instance.UnknownWord;
            typeMap["vdz"] = POSTags.Instance.VBZ;
            typeMap["ajc"] = POSTags.Instance.JJR;
            typeMap["vdn"] = POSTags.Instance.VBN;
            typeMap["vhg"] = POSTags.Instance.VBG;
            typeMap["ajs"] = POSTags.Instance.RB;
            typeMap["pnx"] = POSTags.Instance.PRP;
            typeMap["vdg"] = POSTags.Instance.VBG;
            typeMap["zz0"] = POSTags.Instance.UnknownWord;
            typeMap["vhi"] = POSTags.Instance.UnknownWord;
        }

        public static string TypeToString(this WordType value)
        {
            switch (value)
            {
                default:
                    return "n";
                case WordType.Verb:
                    return "v";
                case WordType.Adjective:
                    return "a";
                case WordType.Adverb:
                    return "r";

            }
        }
        public static WordType ParseType(this string value)
        {
            switch (value.ToLower())
            {
                case "n":
                    return WordType.Noun;
                case "v":
                    return WordType.Verb;
                case "a":
                case "s":
                    return WordType.Adjective;
                case "r":
                    return WordType.Adverb;
                default:
                    return WordType.Unknown;
            }
        }

        public static BasePOSType ParseBNCType(this string value)
        {
            if (typeMap.TryGetValue(value, out BasePOSType baseType))
            {
                return baseType;
            }
            return POSTags.Instance.UnknownWord;
        }

    }
}
