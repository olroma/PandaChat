   M S B A M L   `   `   `  ���� J  FPandaChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PandaChatClientPandaChatClient      PandaChatClient.Auth   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:PandaChatClient     -     � Title$  Авторизация��$	��295��$��613.667�� � WindowStyle$
 None=�.��� ���	#FF1D1D1D$��0,0,0,-1q�.+��� -    ��Name$ button��$��
Войти��$
��Left=�$��329,153,0,0q�$	��Top=�$	��112��$��23��$��PTSansCaption)�$
��Bold"�
����������	#FF28895A K� ZIndex$ -2���� -    ��Name$ label��$��Логин:��$
��Left=�$��	95,81,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
��������� -   $ label1��$%��Пароль к серверу:��$
��Left=�$��	4,244,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
�������G� -    G�Name$ PasswordText��$
��Left=�$��124,249,0,0q�$	��Top=�$��16��$	��184��$��10(�$��PTSansCaption)�$
��Bold"�
����������	#FF1D1D1D�� -    ��Name$ 	LoginText�� �� 	MaxLength$ 12��$
��Left=�$��23��$��
146,84,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��368��	 J� RenderTransformOrigin$	 0.317,-0.087&�$��PTSansCaption)�$
��Bold"����	#FF1D1D1D
�������$��14(��� -   $ label2��$��Авторизация��$
��Left=�$��
245,12,0,0q�$	��Top=�$��18(�$��PTSansCaption)�$
��Bold"�
��������� -   $ MinimasedForm��$��_��$
��Left=�$��	564,0,0,0q�$	��Top=�
��������� -	   $ ExitFormLabel��$��X��$
��Left=�$��	579,0,0,0q�$	��Top=�
�������$��26���� -
   
 ��Name$
 ChoseServer��$
��Left=�$��420,245,0,0q�$	��Top=�$	��163�����	#FF2E2D2D
���   �$��PTSansCaption)�$
��Bold"� �� 
IsReadOnly .  J� OpacityMask �	#FF1D1D1D���	#FF1D1D1D.@��� $+��%Локальный компьютер���� $��МДК 04.01���� $��Свой���� -   $ label3��$ ��Выбор сервера:��$
��Left=�$��317,244,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
��������� -   $ 
IPAuthText�� �� TextAlignment$ Center=�$
��Left=�$��23��$��322,209,0,0q�$
 Wrap=�$	��Top=�$	��175��$��PTSansCaption)�$
��Bold"����	#FF1D1D1D
�������$��	127.0.0.1��{�.  $��14(��� -   $ PortAuthText��$ Center=�$
��Left=�$��23��$��514,209,0,0q�$
 Wrap=�$	��Top=�$��64��$��PTSansCaption)�$
��Bold"����	#FF1D1D1D
�������$
��6666��{�.  $��14(��� -   $ label4��$
��IP: ��$
��Left=�$��322,192,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
�������$	 0.528,0.778&�$��9(��� -   $ label5��$��PORT: ��$
��Left=�$��514,192,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
�������$	 
1.22,0.389&�$��9(�$��20���� -   $ label6��$��:��$
��Left=�$��497,208,0,0q�$	��Top=�
�������$��PTSansCaption)�$
��Bold"��� -   $ VersionLabel��$
��v1.0��$
��Left=�$��
360,24,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
�������$��10(��� -   $ label7��$��Пароль:��$
��Left=�$��
89,116,0,0q�$	��Top=�$��PTSansCaption)�$
��Bold"�
�������G� -   $ PasswordTextUser��$
��Left=�$��22��$��146,118,0,0q�$	��Top=�$	��368�����	#FF1D1D1D
�������$��PTSansCaption)�$��14(��� -   $ 	RegButton��$��Регистрация��$
��Left=�$��177,153,0,0q�$	��Top=�$	��115�����	#FF28895A
�������$��PTSansCaption)�$
��Bold"�$��23���� -   $
 comboBox��$
��Left=�$��
10,210,0,0q�$	��Top=�$	��120��$��PTSansCaption)�$y�Hidden=� J� IsHitTestVisible .  .@���  � Cursor$  ��$	��123���� $
��1234���� $��Свой��