<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyInfo.aspx.cs" Inherits="XQCRSMSystem.ManageSystem.PropertyInfo" %>

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
		
			<h2 class="title">添加信息</h2>
			<div class="layui-row">
				<div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">社区</label>
						<div class="layui-input-block">
                             <asp:DropDownList  name="ddlCommunity" id="ddlCommunity" style=""  runat="server" OnSelectedIndexChanged="ddlCommunity_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
						</div>
					</div>
				</div>
                <div class="layui-col-md4" runat="server" id="dis"  >
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">小区</label>
						<div class="layui-input-block">
                            <asp:DropDownList  name="ddlSubdistrict" id="ddlSubdistrict" style=""  runat="server">
                            </asp:DropDownList>
						</div>
					</div>
				</div>
			</div>
			<div class="layui-row">
				<div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">姓名</label>
						<div class="layui-input-block">
                            <asp:TextBox ID="tbName" runat="server"  CssClass="font"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">账号</label>
						<div class="layui-input-block">
							<asp:TextBox ID="tbUserID" runat="server"  CssClass="font"></asp:TextBox>
						</div>
					</div>
				</div>
			</div>
			<div class="layui-row">
                <div class="layui-col-md4 layui-col-md-offset2">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">手机号</label>
						<div class="layui-input-block">
                            <asp:TextBox ID="tbMobile" runat="server"  CssClass="font"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">密码</label>
						<div class="layui-input-block">
                             <asp:TextBox ID="tbpwd" runat="server"  CssClass="font"></asp:TextBox>
						</div>
					</div>
				</div>
			</div>
			
			 
			 
        <div class="layui-row textc" style="margin-left: 345px;">
                 
             <div class="layui-col-md2 btndiv">
                    <asp:Button ID="btnAdd" runat="server" Text="添加" CssClass="layui-btn layui-btn-sm " OnClick="btnAdd_Click"  OnClientClick="return Check();" />
                </div>
                <div class="layui-col-md2 btndiv">
                    <asp:Button ID="btnRturn" runat="server" Text="返回" CssClass="layui-btn layui-btn-sm " OnClick="btnRturn_Click"/>
                </div>
            </div>
		</div>

    <div>

    </div>
    <asp:HiddenField ID="hidID" runat="server"/>
        <script>

            function Check() {
                //社区
                var select1 = document.all.<%= ddlCommunity.ClientID %>;
                var selectvalue1 = select1.options[select1.selectedIndex].value; 
                if (selectvalue1 == "99")
                {
                    alert('请选择社区！');
                    return false;
                }
                //小区
                var select2 = document.all.<%= ddlSubdistrict.ClientID %>;
                var selectvalue2 = select2.options[select2.selectedIndex].value; 
                if (selectvalue2 == "99")
                {
                    alert('请选择小区！');
                    return false;
                }
                //姓名
                var userName = document.getElementById("tbName").value;
                if (userName == "")
                {
                    alert('请填写姓名！');
                    return false;
                }
                //账号
                var userID = document.getElementById("tbUserID").value;
                if (userID == "")
                {
                    alert('请填写登录账号！');
                    return false;
                }
                //密码
                var pwd = document.getElementById("tbpwd").value;
                if (pwd == "")
                {
                    alert('请填写密码！');
                    return false;
                }
                //判断是否8位以上
                if (pwd.length >= 8) {
                    //判断 是否是字符、数字组合
                    if (checkRate(pwd)) {
                        return true;
                    }
                    else {
                        alert('请填写字符、数字组合密码！');
                        return false;
                    }
                }
                else
                {
                    alert('请填写8为以上字符、数字组合密码！');
                    return false;
                }
            }



        function checkRate(nubmer) {

            var re = /^(([a-z]+[0-9]+)|([0-9]+[a-z]+))[a-z0-9]*$/i; //判断字符串是否为数字和字母组合     //判断正整数 /^[1-9]+[0-9]*]*$/
            if (!re.test(nubmer)) {
                return false;
            } else {
                return true;
            }
        }

        </script>
    </form>
</body>
</html>

