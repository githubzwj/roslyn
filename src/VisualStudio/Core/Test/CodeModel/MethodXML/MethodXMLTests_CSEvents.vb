' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Threading.Tasks
Imports Roslyn.Test.Utilities

Namespace Microsoft.VisualStudio.LanguageServices.UnitTests.CodeModel.MethodXML
    Partial Public Class MethodXMLTests

        <ConditionalWpfFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModelMethodXml)>
        Public Async Function TestCSEvents_AddDelegate() As Task
            Dim definition =
<Workspace>
    <Project Language="C#" CommonReferences="true">
        <Document>
public class C
{
    $$void M()
    {
        this.Foo += Bar;
    }

    private void Bar(object sender, System.EventArgs e) { }

    public event System.EventHandler Foo;
}
        </Document>
    </Project>
</Workspace>

            Dim expected =
<Block>
    <ExpressionStatement line="5">
        <Expression>
            <Assignment binaryoperator="adddelegate">
                <Expression>
                    <NameRef variablekind="field">
                        <Expression>
                            <ThisReference/>
                        </Expression>
                        <Name>Foo</Name>
                    </NameRef>
                </Expression>
                <Expression>
                    <NameRef variablekind="method">
                        <Expression>
                            <ThisReference/>
                        </Expression>
                        <Name>Bar</Name>
                    </NameRef>
                </Expression>
            </Assignment>
        </Expression>
    </ExpressionStatement>
</Block>

            Await TestAsync(definition, expected)
        End Function

        <ConditionalWpfFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModelMethodXml)>
        Public Async Function TestCSEvents_AddDelegateForNonExistentEventHandler1() As Task
            Dim definition =
<Workspace>
    <Project Language="C#" CommonReferences="true">
        <Document>
public class C
{
    $$void M()
    {
        this.Foo += Bar;
    }

    public event System.EventHandler Foo;
}
        </Document>
    </Project>
</Workspace>

            ' Note: The expected result below is slightly different than Dev10 behavior.
            ' In Dev10, the NameRef for Bar will have a variablekind of "local", but
            ' "unknown" is really more correct and shouldn't impact the designer.
            Dim expected =
<Block>
    <ExpressionStatement line="5"><Expression>
        <Assignment binaryoperator="adddelegate">
            <Expression>
                <NameRef variablekind="field">
                    <Expression>
                        <ThisReference/>
                    </Expression>
                    <Name>Foo</Name>
                </NameRef>
            </Expression>
            <Expression>
                <NameRef variablekind="unknown">
                    <Name>Bar</Name>
                </NameRef>
            </Expression>
        </Assignment>
        </Expression>
    </ExpressionStatement>
</Block>

            Await TestAsync(definition, expected)
        End Function

        <ConditionalWpfFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModelMethodXml)>
        Public Async Function TestCSEvents_AddDelegateForNonExistentEventHandler2() As Task
            Dim definition =
<Workspace>
    <Project Language="C#" CommonReferences="true">
        <Document>
public class C
{
    $$void M()
    {
        this.Foo += this.Bar;
    }

    public event System.EventHandler Foo;
}
        </Document>
    </Project>
</Workspace>

            ' Note: The expected result below is slightly different than Dev10 behavior.
            ' In Dev10, the NameRef for Bar will have a variablekind of "property", but
            ' "unknown" is really more correct and shouldn't impact the designer.
            Dim expected =
<Block>
    <ExpressionStatement line="5">
        <Expression>
            <Assignment binaryoperator="adddelegate">
                <Expression>
                    <NameRef variablekind="field">
                        <Expression>
                            <ThisReference/>
                        </Expression>
                        <Name>Foo</Name>
                    </NameRef>
                </Expression>
                <Expression>
                    <NameRef variablekind="unknown">
                        <Expression>
                            <ThisReference/>
                        </Expression>
                        <Name>Bar</Name>
                    </NameRef>
                </Expression>
            </Assignment>
        </Expression>
    </ExpressionStatement>
</Block>

            Await TestAsync(definition, expected)
        End Function

        <ConditionalWpfFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModelMethodXml)>
        Public Async Function TestCSEvents_AddDelegateForNonExistentEventHandler3() As Task
            Dim definition =
<Workspace>
    <Project Language="C#" CommonReferences="true">
        <Document>
public class C
{
    $$void M()
    {
        this.Foo += new System.EventHandler(this.Bar);
    }

    public event System.EventHandler Foo;
}
        </Document>
    </Project>
</Workspace>

            Dim expected =
<Block>
    <ExpressionStatement line="5">
        <Expression>
            <Assignment binaryoperator="adddelegate">
                <Expression>
                    <NameRef variablekind="field">
                        <Expression>
                            <ThisReference/>
                        </Expression>
                        <Name>Foo</Name>
                    </NameRef>
                </Expression>
                <Expression>
                    <NewClass>
                        <Type>System.EventHandler</Type>
                        <Argument>
                            <Expression>
                                <NameRef variablekind="unknown">
                                    <Expression>
                                        <ThisReference/>
                                    </Expression>
                                    <Name>Bar</Name>
                                </NameRef>
                            </Expression>
                        </Argument>
                    </NewClass>
                </Expression>
            </Assignment>
        </Expression>
    </ExpressionStatement>
</Block>

            Await TestAsync(definition, expected)
        End Function

    End Class
End Namespace
