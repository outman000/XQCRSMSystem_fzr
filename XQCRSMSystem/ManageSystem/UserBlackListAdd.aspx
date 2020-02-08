<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserBlackListAdd.aspx.cs" Inherits="XQCRSMSystem.ManageSystem.UserBlackListAdd" %>


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
						<label class="layui-form-label xxtit">姓名</label>
						<div class="layui-input-block">
                            <asp:TextBox ID="tbName" runat="server"  CssClass="font"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="layui-col-md4">
					<div class="layui-form-item">
						<label class="layui-form-label xxtit">身份证号</label>
						<div class="layui-input-block">
							<asp:TextBox ID="tbIDCard" runat="server"  CssClass="font"></asp:TextBox>
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
						<label class="layui-form-label xxtit">备注</label>
						<div class="layui-input-block">
                             <asp:TextBox ID="tbMemo" runat="server"  CssClass="font"></asp:TextBox>
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
               
                //姓名
                var userName = document.getElementById("tbName").value;
                if (userName == "")
                {
                    alert('请填写姓名！');
                    return false;
                }
                //身份账号
                var IDCard = document.getElementById("tbIDCard").value;
                if (IDCard == "")
                {
                    alert('请填写身份账号！');
                    return false;
                }
                 
                //判断身份账号 格式
                if (IdCodeValid(IDCard)) {
                    return true;
                }
                else
                {
                    alert('请填写正确的身份账号！');
                    return false;
                }
            }



        function IdCodeValid(code){
			    //身份证号合法性验证
			    //支持15位和18位身份证号
			    //支持地址编码、出生日期、校验位验证
		    var city={11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江 ",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北 ",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏 ",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外 "};
            var res = true;

            if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|[xX])$/.test(code)) {
                res = false;
		    }else if(!city[code.substr(0,2)]){
		           res = false;
		    }else{
		        //18位身份证需要验证最后一位校验位
		        if(code.length == 18){
		            code = code.split('');
		            //∑(ai×Wi)(mod 11)
		            //加权因子
		            var factor = [ 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 ];
		            //校验位
		            var parity = [ 1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2 ];
		            var sum = 0;
		            var ai = 0;
		            var wi = 0;
		            for (var i = 0; i < 17; i++)
		            {
		                ai = code[i];
		                wi = factor[i];
		                sum += ai * wi;
		            }
		            if(parity[sum % 11] != code[17].toUpperCase()){
		                    res = false;
		            }
		        }
		    }
		    return     res ;
		}
 

        </script>
    </form>
</body>
</html>
