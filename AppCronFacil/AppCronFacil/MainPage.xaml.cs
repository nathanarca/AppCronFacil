using AppCronometroFacil.Model;
using AppCronometroFacil.Pages;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Plugin.SimpleAudioPlayer;
using System;
using Xamarin.Forms;

namespace AppCronometroFacil
{


    public partial class MainPage : ContentPage
    {
        private bool _iniciado;
        private int _intervalo;
        private int _repeticao;
        private int _repeticaoAtual;
        private int _treino;
        private int _contar;
        private bool _pausar;
        private TipoCron _tipo;
        private ISimpleAudioPlayer _player;
        private bool _finalizado;
        private bool _pausado;
        private bool _iniciando;

        public MainPage()
        {
            InitializeComponent();

            AplicarPropriedades();


            if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-5541916824987072/5032238558";
            
            //TESTE
            //if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";

            textBoxDuracao.Unfocused += textBoxUnfocused;
            textBoxIntervalo.Unfocused += textBoxUnfocused;
            textBoxRepeticao.Unfocused += textBoxUnfocused;

            _player = CrossSimpleAudioPlayer.Current;





        }



        private void Contar()
        {
            try
            {

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {

                    if(_finalizado) return false;

                    if (_pausar)
                    {
                        buttonIniciarParar.IsEnabled = false;
                        buttonResetar.IsEnabled = false;
                        _pausado = true;
                        _pausar = false;
                        return true;
                    };

                    if (_pausado)
                    {
                        buttonIniciarParar.IsEnabled = true;
                        buttonResetar.IsEnabled = true;
                        return false;
                    }

                    _contar -= 1;

                    labelCronometro.Text = $"{_contar}";

                    if (_contar <= 3)
                    {
                        _player.Load("tresbeep.mp3");
                        _player.Play();
                    }

                    if (_contar == 0)
                    {
                        _iniciando = false;

                        if (_tipo == TipoCron.Intervalo)
                        {
                            
                            _player.Load("comecarBeep.mp3");
                            _player.Play();
                            _contar = _treino;
                            labelCronometro.Text = $"{_contar}";
                            _tipo = TipoCron.Treino;
                            labelInfo.Text = "exercício";
                            BackgroundColor = Color.FromHex("#e74c3c");

                        }
                        else
                        {

                            if (_repeticao == _repeticaoAtual || _repeticao == 1)
                            {
                                _finalizado = true;
                                labelCronometro.Text = "0";
                                labelInfo.Text = "concluído";
                                BackgroundColor = Color.FromHex("#27ae60");
                                _player.Load("fimBeep.mp3");
                                _player.Play();

                                TreinoFinalizado();

                            }
                            else
                            {
                                
                                _player.Load("comecarBeep.mp3");

                                _player.Play();

                                _tipo = TipoCron.Intervalo;

                                _contar = _intervalo;

                                labelCronometro.Text = $"{_contar}";

                                labelInfo.Text = "descanço";

                                _repeticaoAtual++;

                                labelRepeticao.Text = $"{_repeticaoAtual}/{_repeticao}";

                                BackgroundColor = Color.FromHex("#2980b9");

                            }

                        }
                    }

                    return true;

                });



            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void TreinoFinalizado()
        {
            try
            {
                panelCronometro.IsVisible = false;
                panelFinalizado.IsVisible = true;


                var info = string.Empty;

                info += _repeticao > 1 ? $"{_repeticao} repetições de " : $"{_repeticao} repetição de ";
                info += _treino > 1 ? $" {_treino} segundos com " : $"{_repeticao} segundo com ";
                info += _intervalo > 1 ? $" {_intervalo} segundos de descanço." : $"{_repeticao} segundo de descanço.";

                labelInfoFinalizado.Text = info;

                labelTempoTotal.Text = TextoTempoTotal(_treino, _intervalo, _repeticao);

                _finalizado = true;

                
                buttonIniciarParar.Text = "Enviar";
                buttonIniciarParar.Image = "iconEnviar.png";

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private string TextoTempoTotal(int treino, int intervalo, int repeticao)
        {
            try
            {

                var total = (treino + intervalo) * repeticao;

                int hor = 0;
                int min = 0;
                int seg = 0;


                if (total >= 60)
                {
                    min = total / 60;

                    seg = total - (min * 60);
                }


                if (min >= 60)
                {
                    hor = min / 60;

                    min = min - (hor * 60);

                }

                var texto = string.Empty;

                if (hor > 0)
                {
                    texto += hor > 1 ? $"{hor} horas" : "1 hora";
                }

                if (min > 0)
                {
                    var e = seg > 0 ? "" : " e";
                    texto += min > 1 ? $"{e} {min} minutos" : $"{e} 1 minuto";
                }

                if (seg > 0)
                {
                    texto += seg > 1 ? $" e {seg} segundos" : " e 1 segundo";
                }

                if (string.IsNullOrEmpty(texto))
                {
                    return total > 1 ? $"{total} segundos" : "1 segundo";
                }

                return texto;

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void AplicarPropriedades()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("UltimaRepeticao"))
                {
                    var ultimaRepeticao = (string)Application.Current.Properties["UltimaRepeticao"];

                    textBoxRepeticao.Text = ultimaRepeticao;
                }
                else
                {
                    textBoxRepeticao.Text = @"3";
                }

                if (Application.Current.Properties.ContainsKey("UltimoIntervalo"))
                {
                    var ultimoIntervalo = (string)Application.Current.Properties["UltimoIntervalo"];

                    textBoxIntervalo.Text = ultimoIntervalo;

                }
                else
                {
                    textBoxIntervalo.Text = @"10";
                }


                if (Application.Current.Properties.ContainsKey("UltimaDuracao"))
                {

                    var ultimaDuracao = (string)Application.Current.Properties["UltimaDuracao"];

                    textBoxDuracao.Text = ultimaDuracao;
                }
                else
                {
                    textBoxDuracao.Text = @"30";
                }


                PluralTotal();
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }
        private void SalvarPropriedades()
        {
            try
            {
                Application.Current.Properties["UltimaRepeticao"] = textBoxRepeticao.Text;

                Application.Current.Properties["UltimoIntervalo"] = textBoxIntervalo.Text;

                Application.Current.Properties["UltimaDuracao"] = textBoxDuracao.Text;

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void buttonIniciarParar_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (_finalizado)
                {

                    var message = new ShareMessage
                    {
                        Url = "https://play.google.com/store/apps/details?id=nathanCamara.AppCronometroFacil",
                        Title = "Acabei de concluir um treino com a ajuda do Cron Fácil!",
                        Text = "Acabei de concluir um treino com a ajuda do Cron Fácil!\n" + labelInfoFinalizado.Text + "\n" + labelTempoTotal.Text
                    };

                    CrossShare.Current.Share(message);

                }
                else
                {
                    if (_iniciado)
                    {
                        if (_pausado)
                        {
                            _pausar = false;
                            _pausado = false;

                            if (_iniciando)
                            {
                                labelInfo.Text = @"preparação";
                                BackgroundColor = Color.FromHex("#2980b9");
                            }
                            else
                            {
                                labelInfo.Text = _tipo == TipoCron.Treino ? "exercício" : "descanço";
                                BackgroundColor = _tipo == TipoCron.Treino ? Color.FromHex("#e74c3c") : Color.FromHex("#2980b9");
                            }


                            Contar();


                            buttonIniciarParar.Text = "pausar";
                            buttonIniciarParar.Image = "iconPausa.png";
                        }
                        else
                        {
                            _pausar = true;
                            buttonIniciarParar.IsEnabled = false;
                            buttonResetar.IsEnabled = false;
                            labelInfo.Text = "pausado";
                            BackgroundColor = Color.FromHex("#95a5a6");
                            buttonIniciarParar.Text = "retomar";
                            buttonIniciarParar.Image = "iconPlay.png";
                        }

                    }
                    else
                    {


                        buttonIniciarParar.Text = "PAUSAR";
                        buttonIniciarParar.Image = "iconPausa.png";

                        labelCronometro.Text = textBoxIntervalo.Text;
                        labelInfo.Text = @"preparação";
                        _repeticaoAtual = 1;
                        labelRepeticao.Text = $@"1/{textBoxRepeticao.Text}";

                        _iniciado = true;

                        _intervalo = int.Parse(textBoxIntervalo.Text);

                        _repeticao = int.Parse(textBoxRepeticao.Text);

                        _tipo = TipoCron.Intervalo;

                        _treino = int.Parse(textBoxDuracao.Text);

                        _contar = int.Parse(textBoxIntervalo.Text);

                        BackgroundColor = Color.FromHex("#2980b9");

                        SalvarPropriedades();

                        panelFormulario.IsVisible = false;
                        panelCronometro.IsVisible = true;
                        buttonResetar.IsVisible = true;

                        _pausar = false;
                        _pausado = false;
                        _finalizado = false;
                        _iniciando = true;

                        Contar();

                    }
                }

            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        private void buttonResetar_Clicked(object sender, EventArgs e)
        {

            if (!_iniciado) return;

            

            buttonIniciarParar.Text = "iniciar";
            buttonIniciarParar.FontSize = 20;
            buttonIniciarParar.Image = "iconPlay.png";

            buttonIniciarParar.IsEnabled = _pausado || _finalizado;


            _iniciado = false;
            _pausar = true;
            _finalizado = false;
            panelCronometro.IsVisible = false;
            panelFinalizado.IsVisible = false;
            panelFormulario.IsVisible = true;
            buttonResetar.IsVisible = false;

            BackgroundColor = Color.FromHex("#16a085");


        }

        private async void buttonMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayActionSheet(null, null, null, "Compartilhar", "Sobre");

                switch (action)
                {
                    case "Compartilhar":

                        var message = new ShareMessage
                        {
                            Url = "https://play.google.com/store/apps/details?id=nathanCamara.AppCronometroFacil",
                            Title = "Cron Fácil",
                            Text = "Cron Fácil\nAquela ajudinha básica pra manter o treino na linha!"
                        };

                        await CrossShare.Current.Share(message);

                        break;
                    case "Sobre":

                        await Navigation.PushModalAsync(new PageSobre(), false);

                        break;
                }


            }
            catch (Exception erro)
            {

                throw erro;
            }
        }


        private void buttonRemoveClicked(object sender, EventArgs e)
        {
            try
            {
                var id = ((Button)sender).Id;

                if (id == buttonRemoveIntervalo.Id)
                {
                    Diminuir(textBoxIntervalo);
                }
                else if (id == buttonRemoveRepeticao.Id)
                {
                    Diminuir(textBoxRepeticao);
                }
                else if (id == buttonRemoveDuracao.Id)
                {
                    Diminuir(textBoxDuracao);
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void Diminuir(Entry entry)
        {
            try
            {
                var valorAtual = int.Parse(entry.Text);

                if (valorAtual == 1) return;

                valorAtual -= 1;

                entry.Text = $"{valorAtual}";

                PluralTotal();

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }


        private void Aumentar(Entry entry)
        {
            try
            {

                var valorAtual = int.Parse(entry.Text);

                valorAtual += 1;

                entry.Text = $"{valorAtual}";

                PluralTotal();

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void PluralTotal()
        {
            try
            {
                labelInfoDuracao.Text = int.Parse(string.IsNullOrEmpty(textBoxDuracao.Text.Replace(".", "").Replace(",", "")) ? "1" : textBoxDuracao.Text.Replace(".", "").Replace(",", "")) == 1 ? "segundo" : "segundos";
                labelInfoIntervalo.Text = int.Parse(string.IsNullOrEmpty(textBoxIntervalo.Text.Replace(".", "").Replace(",", "")) ? "1" : textBoxIntervalo.Text.Replace(".", "").Replace(",", "")) == 1 ? "segundo" : "segundos";


                var intervalo = int.Parse(string.IsNullOrEmpty(textBoxIntervalo.Text.Replace(".", "").Replace(",", "")) ? "1" : textBoxIntervalo.Text.Replace(".", "").Replace(",", ""));
                var treino = int.Parse(string.IsNullOrEmpty(textBoxDuracao.Text.Replace(".", "").Replace(",", "")) ? "1" : textBoxDuracao.Text.Replace(".", "").Replace(",", ""));
                var repeticao = int.Parse(string.IsNullOrEmpty(textBoxRepeticao.Text.Replace(".", "").Replace(",", "")) ? "1" : textBoxRepeticao.Text.Replace(".", "").Replace(",", ""));

                labelTempoTotalForm.Text = TextoTempoTotal(treino, intervalo, repeticao);

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void buttonAddClicked(object sender, EventArgs e)
        {
            try
            {
                var id = ((Button)sender).Id;

                if (id == buttonAddIntervalo.Id)
                {
                    Aumentar(textBoxIntervalo);
                }
                else if (id == buttonAddRepeticao.Id)
                {
                    Aumentar(textBoxRepeticao);
                }
                else if (id == buttonAddDuracao.Id)
                {
                    Aumentar(textBoxDuracao);
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void textBoxUnfocused(object sender, FocusEventArgs e)
        {
            try
            {

                var ent = (Entry)sender;

                if (!string.IsNullOrEmpty(ent.Text))
                {
                    ent.Text = ent.Text.Replace(".", "").Replace(",", "");
                }

                if (string.IsNullOrEmpty(ent.Text) || ent.Text == "0")
                {
                    ent.Text = @"1";
                }

                PluralTotal();

            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

    }
}
