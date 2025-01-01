import { motion } from "framer-motion";
import { cn } from "@/shared/utils/utils";
import { Music2, User, Lightbulb, Trophy, Calendar } from 'lucide-react';

interface AnimatedRevealProps {
    title: string;
    artist: string;
    extraInfo?: string;
    points: number;
    isRevealed: boolean;
    year: number;
}

export function AnimatedReveal({ title, artist, extraInfo, points, isRevealed, year }: AnimatedRevealProps) {
    const container = {
        hidden: { opacity: 0 },
        show: {
            opacity: 1,
            transition: {
                staggerChildren: 0.8,
                delayChildren: 0.3
            }
        }
    };

    const item = {
        hidden: { opacity: 0, scale: 0.8, y: 100 },
        show: {
            opacity: 1,
            scale: 1,
            y: 0,
            transition: {
                type: "spring",
                bounce: 0.4,
                duration: 1.2
            }
        }
    };

    const iconVariants = {
        hidden: { scale: 0, rotate: -180 },
        show: {
            scale: 1,
            rotate: 0,
            transition: {
                type: "spring",
                bounce: 0.6,
                duration: 1
            }
        }
    };

    if (!isRevealed) {
        return (
            <div className="text-center text-8xl font-bold text-yellow-400">
                <motion.div
                    initial={{ scale: 1, rotate: 0 }}
                    animate={{
                        scale: [1, 1.2, 1],
                        rotate: [0, 10, -10, 0]
                    }}
                    transition={{
                        duration: 2,
                        repeat: Infinity,
                        ease: "easeInOut"
                    }}
                >
                    ?
                </motion.div>
            </div>
        );
    }

    return (
        <motion.div
            variants={container}
            initial="hidden"
            animate="show"
            className="space-y-12 text-center py-12"
        >
            <motion.div
                variants={item}
                className="relative space-y-2"
            >
                <motion.div variants={iconVariants} className="absolute -left-16 top-1/2 -translate-y-1/2">
                    <Music2 className="w-12 h-12 text-yellow-400" />
                </motion.div>
                <div className="text-2xl text-yellow-400 font-semibold">Song</div>
                <div className="text-6xl font-bold bg-gradient-to-r from-white to-white/80 bg-clip-text text-transparent">
                    {title}
                </div>
            </motion.div>

            <motion.div
                variants={item}
                className="relative space-y-2"
            >
                <motion.div variants={iconVariants} className="absolute -left-16 top-1/2 -translate-y-1/2">
                    <User className="w-12 h-12 text-yellow-400" />
                </motion.div>
                <div className="text-2xl text-yellow-400 font-semibold">Artist</div>
                <div className="text-5xl font-bold text-white/90">{artist}</div>
            </motion.div>

            {/* <motion.div
                variants={item}
                className="relative space-y-2"
            >
                <motion.div variants={iconVariants} className="absolute -left-16 top-1/2 -translate-y-1/2">
                    <Calendar className="w-12 h-12 text-yellow-400" />
                </motion.div>
                <div className="text-2xl text-yellow-400 font-semibold">År</div>
                <div className="text-5xl font-bold text-white/90">{year}</div>
            </motion.div> */}
        </motion.div>
    );
} 