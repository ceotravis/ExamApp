using System.Windows.Forms;

namespace ExamApp {
    public partial class ExamForm : Form {
        private int groupBoxNum = 0;

        private List<string> radioAnswers = new List<string>();
        private List<string> textAnswers = new List<string>();
        private List<string> correctAnswers = new List<string>() { "1or och 0or som används i datorn", "Förstå moral", "Klasser och objekt existerar", "Handlar om att lära datorer att göra specifika uppgifter", "1943", "Nja", "Antalet transistorer i en krets kommer att fördubblas ungefär vartannat år" };

        public ExamForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Compare the user's answer with the correct answer
        /// </summary>
        /// <param name="groupBoxNum">The groupbox where the question resides</param>
        /// <param name="answer">The user's answer</param>
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
        /// Iterate through each GroupBox in the FlowLayout, 
        /// find the selected RadioButton in each group, and add the text of the selected RadioButton to an array.
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
        /// Gets the content of all textboxes in a flowlayoutpanel and adds it to an array together 
        /// with the the label over the textbox
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
        
        /// <summary>
        /// Change to the specified ExamControl tab
        /// </summary>
        /// <param name="tab">The tab to change to</param>
        /// <exception cref="InvalidOperationException">ExamControl needs to be initialized</exception>
        /// <exception cref="ArgumentException">The input string must be a number</exception>
        /// <exception cref="ArgumentOutOfRangeException">You cannot change to a tab that does not exist</exception>
        private void ChangeTab(string tab) {
            if (ExamControl == null) {
                throw new InvalidOperationException("ExamControl is not initialized");
            }

            if (!Int32.TryParse(tab, out int value)) {
                throw new ArgumentException("Cannot convert tab to int");
            }

            if (value < 0 || value >= ExamControl.TabCount) {
                throw new ArgumentOutOfRangeException(nameof(tab), $"{tab} is out of range!");
            }

            ExamControl.SelectTab(value);
        }

        // TODO: Do not force unwrap these

        private void NextPage1_Click(object sender, EventArgs e) {
            ChangeTab(((Button)sender).Tag.ToString()!);
        }

        private void NextPage2_Click(object sender, EventArgs e) {
            ChangeTab(((Button)sender).Tag.ToString()!);
        }

        private void NextPage3_Click(object sender, EventArgs e) {
            ChangeTab(((Button)sender).Tag.ToString()!);
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