<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="Analise.aspx.cs" Inherits="elivro.admin.Analise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>E-Commerce de Livro - Análise</title>

    <script>
        // Area Chart Example
        document.addEventListener('DOMContentLoaded', function () {
            var myChart = Highcharts.chart('container', {

                title: {
                    text: 'Venda de Livros por Categoria Principal'
                },

                subtitle: {
                    text: 'Fonte: Livros'
                },

                yAxis: {
                    title: {
                        text: 'Quantidade de Vendas'
                    }
                },

                xAxis: {
                    <%=xAxis.ToString() %>
                },

                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        }
                    }
                },

                series: [
                    <%=series.ToString() %>
                ],

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }

            });
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">

        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Análise</h1>

        <!-- Content Row -->
        <div class="row">

            <div class="col-xl-12 col-lg-12">

                <!-- Area Chart -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Gráfico de Vendas por Categoria</h6>
                    </div>
                    <div class="card-body">

                        <div class="form-group row">
                            <div class="col-sm-5 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:Label ID="lblDtInicio" runat="server" Text="Data Início:"></asp:Label>
                                    <asp:TextBox ID="txtDtInicio" type="date" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Início"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-5 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:Label ID="lblDtFim" runat="server" Text="Data Fim:"></asp:Label>
                                    <asp:TextBox ID="txtDtFim" type="date" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Fim"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="input-group-append">
                                    <asp:LinkButton ID="btnFiltraGrafico" CssClass="btn btn-primary btn-circle" runat="server" OnClick="btnFiltraGrafico_Click">
                                            <i class="fas fa-fw fa-redo-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <figure class="highcharts-figure">
                            <div id="container"></div>
                        </figure>
                    </div>

                    <div class="card-footer small text-muted" id="lblRodaPeTabela" runat="server"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
