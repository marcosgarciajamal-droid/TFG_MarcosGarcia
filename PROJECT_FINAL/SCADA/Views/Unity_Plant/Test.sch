<?xml version="1.0" encoding="utf-8"?>
<SchemeView title="" xmlns:basic="urn:rapidscada:scheme:basic">
  <Scheme>
    <Version>5.3.1.2</Version>
    <Size>
      <Width>1800</Width>
      <Height>1600</Height>
    </Size>
    <BackColor>White</BackColor>
    <BackImageName />
    <Font>
      <Name>Arial</Name>
      <Size>12</Size>
      <Bold>false</Bold>
      <Italic>false</Italic>
      <Underline>false</Underline>
    </Font>
    <ForeColor>Black</ForeColor>
    <Title />
    <CnlFilter />
  </Scheme>
  <Components>
    <basic:Button>
      <BackColor>RED</BackColor>
      <BorderColor>Blue</BorderColor>
      <BorderWidth>100</BorderWidth>
      <ToolTip>PATATA</ToolTip>
      <ID>1</ID>
      <Name />
      <Location>
        <X>89</X>
        <Y>131</Y>
      </Location>
      <Size>
        <Width>500</Width>
        <Height>500</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor>Yellow</ForeColor>
      <Font>
        <Name>Arial</Name>
        <Size>50</Size>
        <Bold>false</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <ImageName />
      <ImageSize>
        <Width>50</Width>
        <Height>50</Height>
      </ImageSize>
      <Text>START</Text>
      <Action>SendCommandNow</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>133</CtrlCnlNum>
    </basic:Button>
    <basic:Toggle>
      <BackColor>OrangeRed</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>2</ID>
      <Name />
      <Location>
        <X>700</X>
        <Y>208</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>32</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>Yellow</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>101</InCnlNum>
      <CtrlCnlNum>101</CtrlCnlNum>
    </basic:Toggle>
    <basic:Button>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>3</ID>
      <Name />
      <Location>
        <X>725</X>
        <Y>98</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>32</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <ImageName />
      <ImageSize>
        <Width>16</Width>
        <Height>16</Height>
      </ImageSize>
      <Text>STOP</Text>
      <Action>SendCommandNow</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>134</CtrlCnlNum>
    </basic:Button>
    <basic:Button>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>4</ID>
      <Name />
      <Location>
        <X>769</X>
        <Y>282</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <ImageName />
      <ImageSize>
        <Width>16</Width>
        <Height>16</Height>
      </ImageSize>
      <Text>MANUAL</Text>
      <Action>SendCommand</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>148</CtrlCnlNum>
    </basic:Button>
    <basic:Button>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>5</ID>
      <Name />
      <Location>
        <X>735</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <ImageName />
      <ImageSize>
        <Width>16</Width>
        <Height>16</Height>
      </ImageSize>
      <Text>AUTOMATIC</Text>
      <Action>SendCommandNow</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>148</CtrlCnlNum>
    </basic:Button>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>6</ID>
      <Name />
      <Location>
        <X>780</X>
        <Y>475</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>7</ID>
      <Name />
      <Location>
        <X>1070</X>
        <Y>30</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
  </Components>
  <Images />
</SchemeView>