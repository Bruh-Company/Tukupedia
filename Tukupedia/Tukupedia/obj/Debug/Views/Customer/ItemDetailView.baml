   M S B A M L   `   `   `  ����D  @Tukupedia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Tukupedia.Views.CustomerTukupedia.Views.Customer  5   	   -    'Tukupedia.Views.Customer.ItemDetailView   5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:Tukupedia.Views.Customer     5   	   -   5          � WindowStyle$
  None=�5   	    � 
ResizeMode$ NoResize=�5   	      MaterialDesignPaper#���   5
   	     MaterialDesignBody#���  5   	    � Title$ ItemDetailView��5   	   $	��690��6    $
��1100��6-    � WindowStartupLocation$ CenterOwner=�6:   $
��1100��6u   $
��1100��6�   $	��690��6�   $	��690��6�   .��5      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   6,   �� 5      -   5         �Name$ 	cdContent��5      $��3* �60   6=   �� 5      $��1* �6   6,    � RowDefinitions 5   
   �� 5      $��1* �6   .+�5   
   � $��0��6   $��0��6   .+�5      �� $	��Top=�6   .+�5      �� -   5         ��Name$ 	ImageItem��5   .   $��0,100,10.2,0q�6   $	��270��6A   5      5      � 5   
   $��1��6   $��0��6    .+�5      �� $��0,50,0,0q�6    �� Orientation$ Vertical=�6+   .+�5       �� -   5        	 ��Name$	 
tbNamaItem��5    B     textblock-h2#��[ 6   $��Nama Item yang Keren��6V   6t   (  "MaterialDesignThemes.Wpf.RatingBar  5!      -   5        
  Name$
 	RatingBar��5!   +      
IsReadOnly . 
��� ���
   Min$ 1��6d   
   Max$ 5��6l   $��0,4,0,0q�6t   �� 5"      -   5        $	 	tbTerjual��5"   B     textblock-sm#��[ 6   $��
Terjual : ��6U   ���#C8C8C8$��0,4,0,0q�6|   6�   �� 5#      -   5        $	 tbHarga��5#   B     textblock-lg#��[ 6   $��Rp69.000,00��6S   ���#E87059$��0,4,0,0q�6{   6�   �� 5$      
������ 
���   ����	#FFACACAC$	��360��6a   $��16(�6n   $
��Bold"�6|   $��0,15,0,0q�6�    � 	Resources 5%       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName��%W  '��       �� 5&      " ��6    .�5'      ��  � �� 5(   "   �� 5)   &   " ��66   .R�5*   *   �� $��Border��61   $��1,1,1,0q�6?   
�������$ 4,4,0,0��6q   $	��2,0q�6�   ._�5+   .   �� $ ContentSite��6?   $��Center=�5,   )   $��Center=�5-   )   $��Header��5.   )   $��	68,5,68,5q�5/   )   50   +    51   *   P� 52   .    �@��
IsSelected . . �53   2   �� $ Border��69    � �� �#7272726c   54   /   P� 55   .    �@��
IsSelected .  . �56   2   �� $ Border��69    � �� �#3030306c   57   /   59   '   5;      5<      5=      .@�5>      �� -   5        $��tabDescription��5>   A   
���   �$��Description��6j   .��5@      �� .+�5A      �� -	   5        $	 
tbKategori��5A   _   $��5,2,5,2q�6(   #��[ 69   $��Kategori : ��6s   ���#C8C8C86�   �� 5B      -
   5        $	 tbBerat��5B   _   $��5,2,5,2q�6(   #��[ 69   $��Berat : ��6p   ���#C8C8C86�   �� 5C      -   5        $	 tbTitleDescription��5C   _   $��5,2,5,2q�6(   #��[ 69   $��Description : ��6{   ���#C8C8C86�   �� 5D      -   5        $	 tbDescription��5D   a   $��	5,2,5.4,2q�6(   #��[ 6;   $��	Wow Keren��6x   $��14(�6�   ���#C8C8C8$	��247��6�   $��WrapWithOverflow=�6�   5E      5F      �� 5G      -   5        $��	tabReview��5G   <   
���   �$��Review��6`   .��5I      �� $
��Auto=�6'   $
��Auto=�6J   $
��Left=�6o   $��0,0,0,0q�6�   $	��Top=�6�   .��5J      �� -   5         ��Name$ spanelReview��5J   a   $��Center=�6)   $��Center=�6D   5L      5M      5N      �� 5O      -   5        $��tabDiscussion��5O   @   
���   �$��
Discussion��6h   .��5Q      �� $
��Auto��6'   $
��Auto=�64   $
��Auto=�6W   $
��Left=�6|   $��0,0,0,0q�6�   $	��Top=�6�   .��5R      �� -   5        $ spanelDiscussion��5R      $��5,5,5,10q�6)   $	��618��6;   $��Center=�6G   $��Center=�6b   5T      5U      5V      5W      �� 5X      -   5         ��Name$ 
rtbDiskusi��5X   ^   $��74��6r   $��1,1,1,1q�6~   $��16(�6�   . �5Y      /� .T�5Z      J� .*�6$   .*�5[      �� $�	Ask Here!��6"   5\      5]      5^      �� 5_      -   5         ��Name$ btnKirimDiskusi��5_      $��Kirim��6P     btn-primary#��[ 6`   $��Right=�6�   $	��150��6�   5a      5b      � 5c   
   $��2��6   $��0��6   .+�5d      �� $��
0,32,0.4,0q�6   .+�5e      �� -   5        $	 tbJumlah��5e   U   $��	10,43,0,0q�6     textblock-md#��[ 6/   $��Jumlah��6g   6w   �� 5f      $��Center=�6   $
��auto��6:   $��50��6G   $ 
Horizontal=�6S   .+�5g      �� -   5        $ btnMin��5g   4   $��40��6D   $��-��6O   $��Center=�6[   $��Center=�6x   �� 5h      -   5        $	 
tbQuantity��5h       $��	10,0,10,0q�64   #��[ 6G   $��0��6m   $��Center=�6v   $��Center=�6�   �� 5i      -   5        $ btnPlus��5i   5   $��40��6F   $��+��6Q   $��Center=�6]   $��Center=�6z   5j      �� 5k      -   5        $ 
btnAddCart��5k   4   $	��169��6H   $��Add to Cart��6T   #��[ 6j   5l      �� 5m      -   5        $
��Back��5m      $
��Left=�6$   $��	145,0,0,0q�6?   $	��Top=�6R   $��75��6j   
���  ��
���  ��
������� �� IsCancel . 6�   5o      5p      5q      