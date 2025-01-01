import { motion, AnimatePresence } from "framer-motion";
import { useEffect, useState } from "react";
import confetti from 'canvas-confetti';

interface AnimatedScoreProps {
    score: number;
    isActive: boolean;
}

export function AnimatedScore({ score, isActive }: AnimatedScoreProps) {
    const [prevScore, setPrevScore] = useState(score);
    const [isIncreasing, setIsIncreasing] = useState(false);

    useEffect(() => {
        if (score !== prevScore) {
            setIsIncreasing(score > prevScore);
            setPrevScore(score);

            // Shoot confetti for big point gains (5 or more)
            if (score > prevScore && (score - prevScore) >= 5) {
                confetti({
                    particleCount: 100,
                    spread: 70,
                    origin: { y: 0.6 }
                });
            }
        }
    }, [score, prevScore]);

    return (
        <div className="relative">
            <AnimatePresence mode="popLayout">
                <motion.div
                    key={score}
                    initial={{ y: isIncreasing ? 50 : -50, opacity: 0, scale: 0.5 }}
                    animate={{ 
                        y: 0, 
                        opacity: 1, 
                        scale: [0.5, 1.2, 1],
                        rotate: [0, isIncreasing ? 10 : -10, 0]
                    }}
                    exit={{ y: isIncreasing ? -50 : 50, opacity: 0, scale: 0.5 }}
                    transition={{ 
                        type: "spring", 
                        bounce: 0.5, 
                        duration: 0.8,
                        times: [0, 0.6, 1]
                    }}
                    className={`text-6xl font-bold ${isActive ? 'text-yellow-400' : 'text-white'}`}
                >
                    {score}
                </motion.div>
            </AnimatePresence>
            
            {/* Score change indicator */}
            {score !== prevScore && (
                <motion.div
                    initial={{ opacity: 0, x: -20, scale: 0.5 }}
                    animate={{ 
                        opacity: 1, 
                        x: 0, 
                        scale: [0.5, 1.5, 1],
                        y: [0, -20, 0]
                    }}
                    exit={{ opacity: 0, x: 20 }}
                    transition={{ duration: 0.8, times: [0, 0.6, 1] }}
                    className={`absolute -right-16 top-1/2 -translate-y-1/2 text-3xl font-bold ${
                        isIncreasing ? 'text-green-400' : 'text-red-400'
                    }`}
                >
                    {isIncreasing ? '+' : ''}{score - prevScore}
                </motion.div>
            )}

            {/* Celebration effects */}
            {isIncreasing && score !== prevScore && (
                <>
                    {/* Ripple effect */}
                    <motion.div 
                        initial={{ scale: 0.5, opacity: 0 }}
                        animate={{ 
                            scale: [1, 3],
                            opacity: [0.5, 0],
                        }}
                        transition={{ duration: 1, times: [0, 1] }}
                        className="absolute inset-0 bg-yellow-400/20 rounded-full"
                    />
                    
                    {/* Burst effect */}
                    <motion.div
                        initial={{ scale: 1 }}
                        animate={{ 
                            scale: [1, 1.5, 0],
                            opacity: [1, 0.8, 0],
                        }}
                        transition={{ duration: 0.5 }}
                        className="absolute inset-0 bg-yellow-400/40 rounded-full"
                    />
                    
                    {/* Stars effect */}
                    {Array.from({ length: 5 }).map((_, i) => (
                        <motion.div
                            key={i}
                            initial={{ scale: 0, rotate: 0 }}
                            animate={{
                                scale: [0, 1, 0],
                                rotate: [0, 360],
                                x: [0, (i - 2) * 30],
                                y: [0, (i % 2 === 0 ? -1 : 1) * 20]
                            }}
                            transition={{ duration: 0.8, delay: i * 0.1 }}
                            className="absolute inset-0 w-2 h-2 bg-yellow-400"
                            style={{ clipPath: 'polygon(50% 0%, 61% 35%, 98% 35%, 68% 57%, 79% 91%, 50% 70%, 21% 91%, 32% 57%, 2% 35%, 39% 35%)' }}
                        />
                    ))}
                </>
            )}
        </div>
    );
} 