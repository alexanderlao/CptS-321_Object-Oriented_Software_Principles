// Alexander Lao
// 11481444
// 4/25/2017
// CptS 321 - HW14 - Tries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HW14_Alexander_Lao
{
    public partial class Form1 : Form
    {
        private String[] words;
        private string path = @"wordsEn.txt";
        private Trie masterTrie = new Trie();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // import the words from the file
            this.words = File.ReadAllLines(path);

            // loop through all the words
            foreach (String word in words)
            {
                // build the trie
                this.masterTrie.AddString(word);
            }
        }

        // fired when text in the input text box changes
        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            // clear the list box for each new input
            resultListBox.Items.Clear();

            var input = sender as TextBox;
            String userString = input.Text;
            List<String> results = new List<String>(); 

            // build the list of strings that match the prefix
            this.masterTrie.GetPrefixStrings(userString, results);

            // display the results
            resultListBox.Items.AddRange(results.ToArray());
        }
    }

    public class TrieNode
    {
        public char c;
        public List<TrieNode> children;

        public TrieNode(char newChar)
        {
            this.c = newChar;
            this.children = new List<TrieNode>();
        }

        public TrieNode AddOrGetChild(char cc)
        {
            // loop through each node in the list of children
            foreach (TrieNode node in children)
            {
                // if we find matching letters, return the node
                if (node.c == cc) return node;
            }

            // otherwise create a new node for the character
            var newNode = new TrieNode(cc);
            this.children.Add(newNode);
            return newNode;
        }

        public TrieNode GetChild(char cc)
        {
            // loop through each node in the list of children
            foreach (TrieNode node in children)
            {
                // if we find matching letters, return the node
                if (node.c == cc) return node;
            }

            // return null if there's no match
            return null;
        }
    }

    public class Trie
    {
        private TrieNode m_root;

        public Trie()
        {
            // root always contains the null terminating character
            this.m_root = new TrieNode('\0');
        }

        public void AddString(string newString)
        {
            // start at the root
            TrieNode node = this.m_root;

            // loop through each character in the string
            foreach (char character in newString)
            {
                // iterate through the trie to add the character
                node = node.AddOrGetChild(character);
            }

            // add the null terminating character
            node.AddOrGetChild('\0');
        }

        // builds a list of strings in the trie that begin with the given prefix
        public void GetPrefixStrings(String prefix, List<String> results)
        {
            // start at the root
            TrieNode currentNode = this.m_root;

            // loop through each character in the prefix
            foreach (char character in prefix)
            {
                // iterate through the trie
                currentNode = currentNode.GetChild(character);

                // return null if there's no children
                // this means there's no matching prefix
                if (currentNode == null) return;
            }

            // at this point, the list of children for the currentNode
            // contains all of the relevant strings
            TraverseChildren(currentNode, results, prefix);
        }

        public void TraverseChildren(TrieNode currentNode, List<String> matches, String prefix)
        {
            // base case is when the node's char is the null terminator
            if (currentNode.c == '\0')
            {
                // add the prefix string to the matches list
                matches.Add(prefix);
                return;
            }

            // loop through the list of children
            foreach (TrieNode child in currentNode.children)
            {
                // append the child character to the string
                prefix += child.c;

                // make the recursive call
                TraverseChildren(child, matches, prefix);

                // trim off the last character for other results
                prefix = prefix.Substring(0, prefix.Length - 1);
            }
        }
    }
}