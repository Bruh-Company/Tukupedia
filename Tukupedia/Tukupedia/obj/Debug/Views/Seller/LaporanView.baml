   M S B A M L   `   `   `  ����D  @Tukupedia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Tukupedia.Views.SellerTukupedia.Views.Seller  5   	   (    "Tukupedia.Views.Seller.LaporanView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Tukupedia.Views.Seller     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5          � WindowStyle$
  None=�5   	    � 
ResizeMode$ NoResize=�5   	      MaterialDesignPaper#���   5   	     MaterialDesignBody#���  5   	    � Title$  ��5   	   $	��420��6   $	��500��6   $	��500��6+   $	��420��6:   $	��500��6J   $	��420��6Y    � WindowStartupLocation$ CenterScreen=�6i   .��5      � .+�5   
   �� $
��Left=�6   $��	50,30,0,0q�6/   $
��Wrap=�6B   $%��Pilih Tanggal Laporan Penjualan��6V   $	��Top=�6}     textblock-h3-success#��[ 6�   �� 5   
   $
��Left=�6   $��
50,100,0,0q�6/   $
��Wrap=�6C   $��Tanggal Mulai��6W   $	��Top=�6l     textblock-xl-secondary#��[ 6�   (  "System.Windows.Controls.DatePicker  5   
   -   5        $��datePickerMulai��5   v   $
��Left=�6   $��
50,140,0,0q�60   $	��Top=�6D   $	��400��6\   $��18(�6h   6v   �� 5   
   $
��Left=�6   $��
50,200,0,0q�6/   $
��Wrap=�6C   $��Tanggal Akhir��6W   $	��Top=�6l   #��[ 6�     5   
   -   5        $��datePickerAkhir��5   v   $
��Left=�6   $��
50,240,0,0q�60   $	��Top=�6D   $	��400��6\   $��18(�6h   6v   �� 5   
   -   5        $��btnShow��5   �   $��Lihat Laporan��6   $
��Left=�6)   $��
50,300,0,0q�6D   $	��Top=�6X   $	��400��6p     btn-primary#��[ 6|   6�   �� 5   
   -   5        $��Kembali��5      $
��Left=�6#   $��
50,337,0,0q�6>   $	��Top=�6R   $	��400��6j   
���  ��
���  ��
�������6�   5      5      