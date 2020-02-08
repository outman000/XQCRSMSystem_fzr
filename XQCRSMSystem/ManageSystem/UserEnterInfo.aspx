<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEnterInfo.aspx.cs" Inherits="XQCRSMSystem.ManageSystem.UserEnterInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
    <title></title>
    <link href="../css/oacss.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" type="text/css" href="layui/css/layui.css" />
		<link rel="stylesheet" type="text/css" href="css/style.css" />
		<script src="layui/layui.all.js" type=""></script>
        <script  type="text/javascript"  src="../Script/jquery-1.8.0.min.js"></script>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layui-container">
		
			<h2 class="title">登记信息</h2>
			<div class="layui-row">
				<div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">社区</label>
						<div class="layui-input-block">
                            <asp:Label ID="lCommunity" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
                <div class="layui-col-md4" runat="server" id="dis"  >
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">小区</label>
						<div class="layui-input-block">
                            <asp:Label ID="lSubdistrict" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
			</div>
			<div class="layui-row">
				<div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">姓名</label>
						<div class="layui-input-block">
                            <asp:Label ID="lName" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">身份证号</label>
						<div class="layui-input-block">
							<asp:Label ID="lIDcard" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
			</div>
			<div class="layui-row">
                <div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">手机号</label>
						<div class="layui-input-block">
                            <asp:Label ID="lMobile" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">住址</label>
						<div class="layui-input-block">
                            <asp:Label ID="lAddress" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>
			</div>
			
			 
			<div class="layui-row">
                <div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">居民类型</label>
						<div class="layui-input-block">
							<asp:Label ID="lType" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>		
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">登记时间</label>
						<div class="layui-input-block">
							<asp:Label ID="lCreateDate" CssClass="font" runat="server" Width=""></asp:Label>
						</div>
					</div>
				</div>			
			</div>
			
			 
		<h2 class="title">进入信息</h2>
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
                       <asp:TemplateField HeaderText="状态" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lstatus" runat="server" Text='<%#Eval("Status")%>'  ></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" />
                        </asp:TemplateField>
                        
                        <asp:BoundField HeaderText="进入时间" ItemStyle-Width="10%" HeaderStyle-Width="10%" DataField="CreateDate"
                            DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

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
        <div class="layui-row textc" style="margin-left: 345px;">
                 
                <div class="layui-col-md2 btndiv">
                    <asp:Button ID="btnRturn" runat="server" Text="返回" CssClass="layui-btn layui-btn-sm " OnClick="btnRturn_Click"/>
                </div>
            </div>
		</div>

    <div>

    </div>
    <asp:HiddenField ID="hidID" runat="server"/>

    </form>
</body>
</html>

