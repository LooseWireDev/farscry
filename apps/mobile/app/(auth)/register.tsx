import { StyleSheet, TextInput, Pressable } from 'react-native';
import { Link } from 'expo-router';
import { ThemedText } from '@/components/themed-text';
import { ThemedView } from '@/components/themed-view';

export default function RegisterScreen() {
  return (
    <ThemedView style={styles.container}>
      <ThemedText type="title">Create Account</ThemedText>
      <TextInput placeholder="Email" style={styles.input} autoCapitalize="none" keyboardType="email-address" />
      <TextInput placeholder="Password" style={styles.input} secureTextEntry />
      <TextInput placeholder="Confirm Password" style={styles.input} secureTextEntry />
      <Pressable style={styles.button}>
        <ThemedText style={styles.buttonText}>Sign Up</ThemedText>
      </Pressable>
      <Link href="/(auth)/login" style={styles.link}>
        <ThemedText type="link">Already have an account? Sign in</ThemedText>
      </Link>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 24,
    gap: 16,
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
  },
  button: {
    backgroundColor: '#007AFF',
    padding: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontWeight: '600',
    fontSize: 16,
  },
  link: {
    alignSelf: 'center',
    marginTop: 8,
  },
});
