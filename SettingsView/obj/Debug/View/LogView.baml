   M S B A M L   `   `   `  ����G  CSettingsView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SettingsView.ViewSettingsView.View  5      ?$clr-namespace:SettingsView.ResourcesSettingsView.Resources  5          SettingsView.View.LogView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:SettingsView.View     5      :	resources$clr-namespace:SettingsView.Resources     5        � 	Resources  5	      4   .SettingsView.Resources.TypeBackgroundConverter   TypeBackgroundConverter��%   &	          5
   
   6N   5      .��5      ��  �� Orientation$ Vertical=�6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    � MappingMode$ RelativeToBoundingBox=�63    �� 
StartPoint$ 0.5,0&�6W   .��5      �  � Color$ SkyBlue��6    � Offset$ 0Q�60   � 5      $ 	LightGray��6   $	 1.5Q�61   5      .+�5   
   �� $��Stretch=�6    �� TextAlignment$ Center=�62   $��Center=�6I   $��90��6d   $��60(�6p   
���   � J� Opacity$	 0.9Q�5      $��Oblique$�6"   $
��Bold"�66   $��Palatino Linotype)�6H   $��Image-Service Log��6�   ��6g   Z� 	 J� Effect	 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      
   
BlurRadius$
 6Q�6#      ShadowDepth$ 6Q�62   5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��DataGridLog��5         AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    CanUserSortColumns .     CanUserDeleteRows .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .  "   VerticalScrollBarVisibility$ Visible=�5      $	��300��69      RowBackground
 ���� ��. $
��Auto��6�   $��0,0,0,10q�6�      	RowHeight$ 30Q�6�   
������ ��5   &   �� *	VM_Logs+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Type��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6N   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Status+   ElementStyle 5      �� 5       �� 
TargetType" ��6    .�5       ��  �� Property � �� �� Value 6;   �� *Status+ �� 	Converter# [  6�   �� 5!       � ��$ 12(�69   �� 5"       � ��$ Stretch=�6D   6V   �� 5#       � ��$ Kristen ITC)�6;   �� 5$       �@��TextAlignment$ Center=�6>   6O   5%      5'        5(      $ Message��6%   $ * 6R    66   �� *	Message+ 5)      �� 5*      " ��6    .�5+      ��  � ��
 ���� 6Q   �� 5,       � ��$ 12(�69   5-      5/      51      52      53      