﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using OLEDB.Test.ModuleCore;

namespace XmlReaderTest.Common
{
    public partial class TCXmlns : TCXMLReaderBaseGeneral
    {
        // Type is XmlReaderTest.Common.TCXmlns
        // Test Case
        public override void AddChildren()
        {
            // for function TXmlns1
            {
                this.AddChild(new CVariation(TXmlns1) { Attribute = new Variation("Name, LocalName, Prefix and Value with xmlns=ns attribute") { Pri = 0 } });
            }


            // for function TXmlns2
            {
                this.AddChild(new CVariation(TXmlns2) { Attribute = new Variation("Name, LocalName, Prefix and Value with xmlns:p=ns attribute") });
            }


            // for function TXmlns3
            {
                this.AddChild(new CVariation(TXmlns3) { Attribute = new Variation("LookupNamespace with xmlns=ns attribute") });
            }


            // for function TXmlns4
            {
                this.AddChild(new CVariation(TXmlns4) { Attribute = new Variation("MoveToAttribute access on xmlns attribute") });
            }


            // for function TXmlns5
            {
                this.AddChild(new CVariation(TXmlns5) { Attribute = new Variation("GetAttribute access on xmlns attribute") });
            }


            // for function TXmlns6
            {
                this.AddChild(new CVariation(TXmlns6) { Attribute = new Variation("this[xmlns] attribute access") });
            }
        }
    }
}
