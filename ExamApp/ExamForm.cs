using System.Windows.Forms;

namespace ExamApp {
    public partial class ExamForm : Form {
        private int groupBoxNum = 0;

        private List<String> radioAnswers = new List<String>();
        private List<String> textAnswers = new List<String>();
        private List<String> correctAnswers = new List<String>() { "1or och 0or som används i datorn", "Förstå moral", "Klasser och objekt existerar", "Handlar om att lära datorer att göra specifika uppgifter", "1943", "Nja", "Antalet transistorer i en krets kommer att fördubblas ungefär vartannat år" };

        public ExamForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the answer from the groupbox and compares it with a list of correct answers.
        /// </summary>
        /// <param name="groupBoxNum">The groupbox where the question resides</param>
        /// <param name="answer">The user's answers</param>
        /// <returns></returns>
        private string CheckAndCompareAnswer(int groupBoxNum, string answer) {
            try {
                if (answer == correctAnswers[groupBoxNum]) {
                    return "Rätt";
                } else {
                    return "Fel";
                }
            } catch {
                return "Det rätta svaret har inte lagts till";
            }
        }

        /// <summary>
        /// Gets the selected radiobutton from all groupboxes in a flowlayout and adds the radiobutton's text to an array
        /// </summary>
        /// <param name="flowLayout">The parent containing the groupboxes</param>
        /// <returns></returns>
        private async Task GetRadioChecked(FlowLayoutPanel flowLayout) {
            await Task.Run(() => {
                foreach (Control control in flowLayout.Controls.OfType<GroupBox>().ToList()) {
                    foreach (RadioButton radio in control.Controls.OfType<RadioButton>().ToList()) {
                        if (radio.Checked) {
                            radioAnswers.Add($"{control.Text}\n{radio.Text}: {CheckAndCompareAnswer(groupBoxNum, radio.Text)}");
                            groupBoxNum++;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Gets the contents of all textboxes in a flowlayoutpanel and adds it to an array together 
        /// with the the label over it
        /// </summary>
        /// <param name="flowLayout"></param>
        private void GetTextAnswer(FlowLayoutPanel flowLayout) {
            foreach (Label label in flowLayout.Controls.OfType<Label>().ToList()) {
                foreach (RichTextBox textBoxA in flowLayout.Controls.OfType<RichTextBox>().ToList()) {
                    if (textBoxA.Text != "") {
                        textAnswers.Add($"{label.Text}\n{textBoxA.Text}");
                    }
                }
            }
        }

        private void nextPage1_Click(object sender, EventArgs e) {
            ExamControl.SelectTab(1);
        }

        private void nextPage2_Click(object sender, EventArgs e) {
            ExamControl.SelectTab(2);
        }

        private void nextPage3_Click(object sender, EventArgs e) {
            ExamControl.SelectTab(3);
        }

        private async void turnIn_Click(object sender, EventArgs e) {
            // Get all of the user's answers 
            await GetRadioChecked(pageFlow1);
            await GetRadioChecked(pageFlow2);
            await GetRadioChecked(pageFlow3);
            await GetRadioChecked(pageFlow4);
            GetTextAnswer(pageFlow1);
            GetTextAnswer(pageFlow2);
            GetTextAnswer(pageFlow3);
            GetTextAnswer(pageFlow4);

            // The user's answers will be added to a test.txt file
            StreamWriter sw = new StreamWriter("test.txt");

            // Write the contents of radioAnswers to the text file
            foreach (string radioA in radioAnswers) {
                sw.WriteLine(radioA);
            }

            // Write the contents of textAnswers to the text file
            foreach (string textA in textAnswers) {
                sw.WriteLine(textA);
            }

            sw.Close();

            MessageBox.Show("Svaren har lämnats in", "Lämna in", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void xkcdLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ImageView imageView = new ImageView();
            imageView.Show();
        }
    }
}