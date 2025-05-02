import { motion } from 'framer-motion';

export function AnimationTest() {
  return (
    <motion.div
      initial={{ opacity: 0, y: -50 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
    >
      <h1>Welcome to the Quiz Room!</h1>
    </motion.div>
  );
}