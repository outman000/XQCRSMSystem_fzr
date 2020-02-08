<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfoList.aspx.cs" Inherits="XQCRSMSystem.ManageSystem.UserInfoList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="My97DatePicker/WdatePicker.js"></script>
    <script src="../Script/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Script/jquery.ui.pager.1.0.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="layui/css/layui.css"/>
    <link rel="Stylesheet" type="text/css" href="css/style.css"/>
    <style>
        .layui-btn {
        width:70%;
        }
        .layui-div label {
text-align:center;
}
        #ddlCurrentPage {
           width: 40px;
    height: 28px;
    margin: 0 10px;
        }
        .layui-input.chengdate {
        display:inline-block}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layui-container">

            <h2 class="title">登记信息列表</h2>
            <div class="layui-row">
                 <div class="layui-col-md3">
                    <div class="layui-form-item">
                        <label class="layui-form-label">社区</label>
                        <div class="layui-input-block">
                            <asp:DropDownList  name="ddlCommunity" id="ddlCommunity" style=""  runat="server" OnSelectedIndexChanged="ddlCommunity_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="layui-col-md3">
                    <div class="layui-form-item">
                        <label class="layui-form-label">小区</label>
                        <div class="layui-input-block">
                            <asp:DropDownList  name="ddlSubdistrict" id="ddlSubdistrict" style=""  runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="layui-col-md3">
                    <div class="layui-form-item">
                        <label class="layui-form-label">关键词</label>
                        <div class="layui-input-block">
                            <asp:TextBox ID="tbKeys"  runat="server" CssClass="layui-input"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="layui-col-md3">
                    <div class="layui-form-item">
                        <label class="layui-form-label">居民类型</label>
                        <div class="layui-input-block">
                            <asp:DropDownList  name="ddlType" id="ddlType" style=""  runat="server">
                                <asp:ListItem Value="99">-请选择-</asp:ListItem>
                                <asp:ListItem Value="0">常住</asp:ListItem>
                                <asp:ListItem Value="1">访客</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="layui-col-md5">
					<div class="layui-form-item">
						<label class="layui-form-label">登记时间</label>
						<div class="layui-input-block">
							<asp:TextBox ID="tbStime" CssClass="layui-input chengdate" Width="160" onclick="WdatePicker({ el: this, dateFmt: 'yyyy-MM-dd' })" runat="server" ></asp:TextBox> 
                            -
							<asp:TextBox ID="tbEtime" CssClass="layui-input chengdate" Width="160" onclick="WdatePicker({ el: this, dateFmt: 'yyyy-MM-dd' })" runat="server" ></asp:TextBox>
						</div>
                    </div>
				</div>
			</div>
         </div>
            <div class="layui-row textc" style="margin-left: 245px;">
                <div class="layui-col-md2 btndiv">
                    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="layui-btn  layui-btn-sm " OnClick="btnSearch_Click" />
                </div>
                <div class="layui-col-md2 btndiv">
                    <asp:Button ID="btnExcel" runat="server"  Text="导出" CssClass="layui-btn layui-btn-sm " OnClick="btnExcel_Click" />
                </div>
            </div>

           
           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="layui-table" BorderColor="ActiveCaption"
                    PageSize="15" AllowPaging="true"  BorderStyle="Solid" OnRowDataBound="GridView1_RowDataBound"
                    Width="100%" HeaderStyle-HorizontalAlign="Center">
                    <PagerSettings Visible="False" />
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# (Container.DataItemIndex+1).ToString()%>
                            </ItemTemplate>
                            <HeaderStyle Width="4%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="社区" ItemStyle-Width="8%" HeaderStyle-Width="8%"
                            DataField="Community" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="小区" ItemStyle-Width="8%" HeaderStyle-Width="8%"
                            DataField="Subdistrict" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="姓名" ItemStyle-Width="6%" HeaderStyle-Width="6%" DataField="Name"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="身份证号" ItemStyle-Width="8%" HeaderStyle-Width="8%" DataField="CertificateID"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="手机号" ItemStyle-Width="6%" HeaderStyle-Width="6%" DataField="Mobile"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="详细住址" ItemStyle-Width="14%" HeaderStyle-Width="14%" DataField="Address"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />   
                        <asp:TemplateField HeaderText="居民类型" ItemStyle-Width="8%" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="ltype" runat="server" Text='<%#Eval("ResidentOrVisitor")%>'  ></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="登记时间" ItemStyle-Width="10%" HeaderStyle-Width="10%" DataField="CreateDate"
                            DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="查看进入信息" ItemStyle-Width="8%" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lID" runat="server" Text='<%#Eval("ID")%>'  Visible="false" ></asp:Label>
                                <asp:LinkButton ID="LDetail" OnClick="LDetail_Click"  style='color: Blue' runat="server">查看进入信息</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle Height="22px" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#D8ECF5" Font-Size="15px" Font-Bold="True" ForeColor="Black"
                        Height="30px" Font-Names="微软雅黑" HorizontalAlign="Center" />
                    <EditRowStyle BackColor="#999999" BorderColor="Gray" />
                    <AlternatingRowStyle ForeColor="Black" />
                </asp:GridView>
    
            <div class="table_box_data">
                
                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
                    border: none;">
                    <tr>
                        <td align="center">
                            <span style="">&nbsp;
                                <asp:LinkButton ID="lnkbtnFrist" runat="server" OnClick="lnkbtnFrist_Click">首页</asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnPre" runat="server" OnClick="lnkbtnPre_Click">上一页</asp:LinkButton>
                                <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkbtnNext" runat="server" OnClick="lnkbtnNext_Click">下一页</asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnLast" runat="server" OnClick="lnkbtnLast_Click">尾页</asp:LinkButton>
                                跳转到第<asp:DropDownList ID="ddlCurrentPage" Width="" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                </asp:DropDownList>
                                页&nbsp; </span>
                        </td>
                    </tr>
                </table>
            </div>
  

    </form>
</body>
</html>
