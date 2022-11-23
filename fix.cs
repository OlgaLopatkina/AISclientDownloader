Подключается к порталу фнс  и получает актуальные версии клиентов  АИС 
private void GetAISVersions(string user, string pwd)
        {

           
            Dispatcher.BeginInvoke(new ThreadStart(delegate { tbAISver.Text = ""; }));


            Trace.WriteLine(user);
            logger.Info("Получаю версию АИС клиентов с https://support.tax.nalog.ru/");

            //Dispatcher.BeginInvoke(new ThreadStart(delegate { pwd = pwdbox.Password; }));
            string url = "https://support.tax.nalog.ru/?login=yes";
            string data = $"backurl=%2F&AUTH_FORM=Y&TYPE=AUTH&USER_LOGIN={user}&USER_PASSWORD={pwd}&Login=%D0%92%D0%BE%D0%B9%D1%82%D0%B8";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            CookieContainer cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                //простая проерка  страницы на  доступность. аналог пинга
                HttpResponseMessage response = client.GetAsync(url + data).Result;
                if (response.StatusCode == (HttpStatusCode)200)
                {

                    Trace.WriteLine("Страница  доступна\n");
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { tbAISver.Text = "Проверка версий АИС."; }));


                }
                else
                {
                    logger.Error("страница недоступна. Провете  пароль или учетка  может быть заблокирована");
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { tbAISver.Text = "Страница недоступна. Проверьте логин/пароль или сам сайт."; }));
                }

                //подключаемся к странице
                //parsing
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;

                HttpWebResponse response2 = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response2.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
                string answer = streamReader.ReadToEnd();

                //System.Windows.Forms.MessageBox.Show("страница недоступна. Проверьте логин/пароль или сам сайт.");
                //страница с фиксами


                //parsing
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(answer);

                var divContainer = document.DocumentNode.SelectNodes("//div[@class='curversion']");



                if (divContainer== null)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { tbAISver.Text = "Ошибка авторизации"; }));
                    return;
                }
                var versionsAIS = divContainer[0].InnerText.Trim();
                var newver = versionsAIS.Replace("&nbsp;", "");
                Trace.WriteLine(newver);

                Dispatcher.BeginInvoke(new ThreadStart(delegate { tbAISver.Text = newver; }));
                Dispatcher.BeginInvoke(new ThreadStart(delegate { btnaisversion.IsEnabled = true; }));


            }
        }
