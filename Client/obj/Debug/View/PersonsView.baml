   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Client.ViewClient.View  5      3clr-namespace:Client.ViewModelClient.ViewModel  5          Client.View.PersonsView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:Client.View     5      -vmclr-namespace:Client.ViewModel     5      S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5      #   Client.ViewModel.BoolToString   boolToString'   !Client.ViewModel.BoolToVisibility  boolToVisibility��%:   &	        &	        5   
   6     5   
   6   5      .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   ]� $��Stretch=�6   .+�5      �� $
��Left=�6   $
��Left=�6+   $��Center=�6G   $	��135��6�   $��Update account��6�   ��6b   �� *EditPersonCommand+6�   5      &   System.Windows.Controls.DataGrid  5   
      CanUserSortColumns .    CanUserAddRows .     AutoGenerateColumns .  "   VerticalScrollBarVisibility$
 Auto=�5      $	��300��5      -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 13 8 8 8q�5         ColumnHeaderPadding$ 8q�5         
IsReadOnly . ��5      �� *	Persons+	   Columns	 5      9  3MaterialDesignThemes.Wpf.MaterialDataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Username��5         	MaxLength$	 255��5      1  +System.Windows.Controls.DataGridBoundColumn   Binding 5      �� *
Username+5        5!      $

 Name��5#      $	 255��5$       5"      �� *Name+5$        5&      $
 	Last name��5(      $	 255��5)       5'      �� *
LastName+5)        5+      $
 	User type��5-      $	 255��5.       5,      �� *	IsAdmin+ �� 	Converter# [  5.      51      52      53      