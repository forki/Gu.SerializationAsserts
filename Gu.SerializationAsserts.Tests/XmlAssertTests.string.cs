﻿namespace Gu.SerializationAsserts.Tests
{
    using System;

    using NUnit.Framework;

    public partial class XmlAssertTests
    {
        [Test]
        public void HappyPath()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                      "  <Outer Attribute=\"meh\">" +
                      "    <Value Attribute=\"1\">2</Value>" +
                      "  </Outer>  " +
                      "</Dummy>";
            XmlAssert.Equal(xml, xml);
        }

        [Test]
        public void EqualWhenArrayVerbatim()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                      "<ArrayOfDummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "  <Dummy>\r\n" +
                      "    <Value>1</Value>\r\n" +
                      "  </Dummy>\r\n" +
                      "  <Dummy>\r\n" +
                      "    <Value>1</Value>\r\n" +
                      "  </Dummy>\r\n" +
                      "</ArrayOfDummy>";
            XmlAssert.Equal(xml, xml, XmlAssertOptions.Verbatim);
        }

        [Test]
        public void EqualWhenVerbatim()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                      "  <Outer Attribute=\"meh\">" +
                      "    <Value Attribute=\"1\">2</Value>" +
                      "  </Outer>  " +
                      "</Dummy>";
            XmlAssert.Equal(xml, xml);
            XmlAssert.Equal(xml, xml, XmlAssertOptions.Verbatim);
        }

        [Test]
        public void EqualWhenIgnoreDeclaration()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                           "  <Outer Attribute=\"meh\">" +
                           "    <Value Attribute=\"1\">2</Value>" +
                           "  </Outer>  " +
                           "</Dummy>";

            var actual =
                "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                "  <Outer Attribute=\"meh\">" +
                "    <Value Attribute=\"1\">2</Value>" +
                "  </Outer>  " +
                "</Dummy>";

            XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreDeclaration);
        }

        [Test]
        public void NotEqualWhenMissingDeclaration()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual =
                "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                "  <Outer Attribute=\"meh\">\r\n" +
                "    <Value Attribute=\"1\">2</Value>\r\n" +
                "  </Outer>\r\n" +
                "</Dummy>";

            var ext = Assert.Throws<AssertException>(() => XmlAssert.Equal(expected, actual));
            var em = "  Xml differ at line 1 index 1.\r\n" +
                     "  Expected: 1| <?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                     "  But was:  1| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                     "  --------------^";
            Console.WriteLine(ext.Message);
            Assert.AreEqual(em, ext.Message);
        }

        [Test]
        public void EqualWhenIgnoreNamespaces()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                         "<Dummy>\r\n" +
                         "  <Outer Attribute=\"meh\">\r\n" +
                         "    <Value Attribute=\"1\">2</Value>\r\n" +
                         "  </Outer>\r\n" +
                         "</Dummy>";

            XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreNamespaces);
        }

        [Test]
        public void EqualWhenDifferentCaseEncoding()
        {
            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Dummy />";
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Dummy />";
            XmlAssert.Equal(expectedXml, actualXml);
        }

        [Test]
        public void EqualWhenEmptyElements()
        {
            var xml1 = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                      "    <Value></Value>" +
                      "</Dummy>";

            var xml2 = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
          "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
          "    <Value />" +
          "</Dummy>";
            XmlAssert.Equal(xml1, xml2, XmlAssertOptions.IgnoreOrder);
            XmlAssert.Equal(xml1, xml2, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
            XmlAssert.Equal(xml1, xml2);
            XmlAssert.Equal(xml2, xml1);
            XmlAssert.Equal(xml1, xml2, XmlAssertOptions.Verbatim);
            XmlAssert.Equal(xml2, xml1, XmlAssertOptions.Verbatim);
        }

        [Test]
        public void NotEqualWhenWrongEncoding()
        {
            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Dummy />";
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Dummy />";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 1 index 34.\r\n" +
                           "  Expected: 1| <?xml version=\"1.0\" encoding=\"utf-16\"?><Dummy />\r\n" +
                           "  But was:  1| <?xml version=\"1.0\" encoding=\"utf-8\"?><Dummy />\r\n" +
                           "  -----------------------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongRoot()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Wrong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "</Wrong>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 2 index 1.\r\n" +
                           "  Expected: 2| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  But was:  2| <Wrong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongRootIgnoreDeclaration()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy>\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<Wrong>\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "</Wrong>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreDeclaration));
            var expected = "  Xml differ at line 2 index 1.\r\n" +
                           "  Expected: 2| <Dummy>\r\n" +
                           "  But was:  1| <Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNamespaces()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                         "<Dummy>\r\n" +
                         "  <Outer Attribute=\"meh\">\r\n" +
                         "    <Value Attribute=\"1\">2</Value>\r\n" +
                         "  </Outer>\r\n" +
                         "</Dummy>";

            var ex = Assert.Throws<AssertException>(() => XmlAssert.Equal(expected, actual, XmlAssertOptions.Verbatim));

            var em = "  Xml differ at line 2 index 6.\r\n" +
                     "  Expected: 2| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                     "  But was:  2| <Dummy>\r\n" +
                     "  -------------------^";
            Assert.AreEqual(em, ex.Message);
        }

        [Test]
        public void NotEqualWhenInvalidXmlStartingWithWhiteSpace()
        {
            var xml = " <?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "  <Value>2</Value>\r\n" +
                      "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(xml, xml));
            var expected = "  expected is not valid xml.\r\n" +
                           "  XmlException: Unexpected XML declaration. The XML declaration must be the first node in the document, and no white space characters are allowed to appear before it. Line 1, position 4.";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenInvalidXmlUnmatchedElement()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                      "<Dummy>\r\n" +
                      "  <Value>2</Wrong>\r\n" +
                      "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(xml, xml));
            var expected = "  expected is not valid xml.\r\n" +
                           "  XmlException: The 'Value' start tag on line 3 position 4 does not match the end tag of 'Wrong'. Line 3, position 13.";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongElement()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Wrong>2</Wrong>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 3 index 1.\r\n" +
                           "  Expected: 3| <Value>2</Value>\r\n" +
                           "  But was:  3| <Wrong>2</Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenMissingElement1()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "  <Element>3</Element>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Element at line 4 in expected not found in actual.\r\n" +
                           "  Expected: 4| <Element>3</Element>\r\n" +
                           "  But was:  ?| Missing\r\n" +
                           "  ----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenMissingElement2()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "  <Element>3</Element>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Element at line 4 in actual not found in expected.\r\n" +
                           "  Expected:  | No element\r\n" +
                           "  But was:  4| <Element>3</Element>\r\n" +
                           "  ----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenEmptyAndMissingElement1()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                               "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                               "  <Value></Value>\r\n" +
                               "</Dummy>";

            var actualXmls = new[]
                                 {
                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                     "</Dummy>",

                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                                 };

            foreach (var actualXml in actualXmls)
            {
                var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
                var expected = "  Element at line 3 in expected not found in actual.\r\n" +
                               "  Expected: 3| <Value></Value>\r\n" +
                               "  But was:  ?| Missing\r\n" +
                               "  ----------^";
                Assert.AreEqual(expected, xmlExt.Message);
            }
        }

        [Test]
        public void NotEqualWhenEmptyAndMissingElement2()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value />\r\n" +
                              "</Dummy>";

            var actualXmls = new[]
                                 {
                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                     "</Dummy>",

                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                                 };
            foreach (var actualXml in actualXmls)
            {
                var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
                var expected = "  Element at line 3 in expected not found in actual.\r\n" +
                               "  Expected: 3| <Value />\r\n" +
                               "  But was:  ?| Missing\r\n" +
                               "  ----------^";
                Assert.AreEqual(expected, xmlExt.Message);
            }
        }

        [Test]
        public void NotEqualWhenEmptyAndMissingElement3()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Element />\r\n" +
                              "  <Value />\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Element />\r\n" +
                                     "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
            var expected = "  Element at line 4 in expected not found in actual.\r\n" +
                           "  Expected: 4| <Value />\r\n" +
                           "  But was:  ?| Missing\r\n" +
                           "  ----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void EqualWhenTreatEmptyAndMissingElement4()
        {
            var xml1 = @"<Positions><C></C><X>2.2</X><Y>3.3</Y><E/></Positions>";
            var xml2 = @"<Positions><X>2.2</X><D/><Y>3.3</Y></Positions>";
            Assert.Throws<AssertException>(() => XmlAssert.Equal(xml1, xml2));
            Assert.Throws<AssertException>(() => XmlAssert.Equal(xml2, xml1));
            Assert.Throws<AssertException>(() => XmlAssert.Equal(xml1, xml2));
            Assert.Throws<AssertException>(() => XmlAssert.Equal(xml2, xml1));
        }

        [Test]
        public void EqualTreatEmptyAndMissingElementsAsEqual1()
        {
            var expectedXmls = new[]
                                   {
                                      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Value></Value>\r\n" +
                                      "</Dummy>",

                                      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Value />\r\n" +
                                      "</Dummy>",
                                   };

            var actualXmls = new[]
                                 {
                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                     "</Dummy>",

                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                                 };
            foreach (var expectedXml in expectedXmls)
            {
                foreach (var actualXml in actualXmls)
                {
                    XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
                    XmlAssert.Equal(actualXml, expectedXml, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
                    XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
                    XmlAssert.Equal(actualXml, expectedXml, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
                }
            }
        }

        [Test]
        public void EqualWhenTreatEmptyAndMissingElement2()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <X>1.2</X>\r\n" +
                              "  <Value />\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <X>1.2</X>\r\n" +
                                     "</Dummy>";

            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
            XmlAssert.Equal(actualXml, expectedXml, XmlAssertOptions.TreatEmptyAndMissingAsEqual);

            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
            XmlAssert.Equal(actualXml, expectedXml, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
        }

        [Test]
        public void EqualWhenTreatEmptyAndMissingElement3()
        {
            var xml1 = @"<Positions><C></C><X>2.2</X><Y>3.3</Y><E/></Positions>";
            var xml2 = @"<Positions><X>2.2</X><D/><Y>3.3</Y></Positions>";
            XmlAssert.Equal(xml1, xml2, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
            XmlAssert.Equal(xml2, xml1, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
            XmlAssert.Equal(xml1, xml2, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
            XmlAssert.Equal(xml2, xml1, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
        }

        [Test]
        public void NotEqualWhenWrongNestedElement()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer>\r\n" +
                              "    <Value>2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer>\r\n" +
                            "    <Wrong>2</Wrong>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 4 index 1.\r\n" +
                           "  Expected: 4| <Value>2</Value>\r\n" +
                           "  But was:  4| <Wrong>2</Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongElementOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1>1</Value1>\r\n" +
                              "  <Value2>2</Value2>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value2>2</Value2>\r\n" +
                            "  <Value1>1</Value1>\r\n" +
                            "</Dummy>";

            var exts = new[]
                           {
                               Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml)),
                               Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim))
                           };
            var expected = "  The order of elements is incorrect.\r\n" +
                           "  Line 4 in expected is found at line 3 in actual.\r\n" +
                           "  Expected: 4| <Value2>2</Value2>\r\n" +
                           "  But was:  3| <Value2>2</Value2>\r\n" +
                           "  ----------^";
            foreach (var ext in exts)
            {
                Assert.AreEqual(expected, ext.Message);
            }
        }

        [Test]
        public void EqualIgnoreElementOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1>1</Value1>\r\n" +
                              "  <Value2>2</Value2>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value2>2</Value2>\r\n" +
                            "  <Value1>1</Value1>\r\n" +
                            "</Dummy>";
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreElementOrder);
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreOrder);
        }

        [Test]
        public void NotEqualWhenWrongAttributeOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                            "</Dummy>";

            var ex1 = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  The order of attributes is incorrect.\r\n" +
                           "  Xml differ at line 3 index 17.\r\n" +
                           "  Expected: 3| <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                           "  But was:  3| <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                           "  ------------------------------^";

            Assert.AreEqual(expected, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
            Assert.AreEqual(expected, ex2.Message);
        }

        [Test]
        public void EqualIgnoreAttributeOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                            "</Dummy>";

            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreAttributeOrder);
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreOrder);
        }

        [Test]
        public void NotEqualWhenWrongNestedElementValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Attribute=\"1\">Wrong</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 4 index 21.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Attribute=\"1\">Wrong</Value>\r\n" +
                           "  ----------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNestedAttribute()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Wrong=\"1\">2</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 4 index 7.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Wrong=\"1\">2</Value>\r\n" +
                           "  --------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNestedAttributeValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Attribute=\"Wrong\">2</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 4 index 18.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Attribute=\"Wrong\">2</Value>\r\n" +
                           "  -------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongElementValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>1</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>Wrong</Value>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Xml differ at line 3 index 7.\r\n" +
                           "  Expected: 3| <Value>1</Value>\r\n" +
                           "  But was:  3| <Value>Wrong</Value>\r\n" +
                           "  --------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }
    }
}
