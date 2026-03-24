import React, { useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { base44 } from '@/api/base44Client';
import QuickAddSection from '@/components/baby/QuickAddSection';
import HistorySection from '@/components/baby/HistorySection';
import { Loader2, UserCircle } from 'lucide-react';
import UpcomingTasks from '@/components/home/UpcomingTasks';
import { useNavigate } from 'react-router-dom';
import { createPageUrl } from '@/utils';



    export default function Baby() {
        const navigate = useNavigate();
    const queryClient = useQueryClient();
    const [editingActivity, setEditingActivity] = useState(null);

    const { data: activities = [], isLoading } = useQuery({
        queryKey: ['babyActivities'],
        queryFn: () => base44.entities.BabyActivity.list('-timestamp', 200),
    });

    const handleActivityAdded = () => {
        queryClient.invalidateQueries({ queryKey: ['babyActivities'] });
        setEditingActivity(null);
    };

    const handleEditActivity = (activity) => {
        setEditingActivity(activity);
        window.scrollTo({ top: 0, behavior: 'smooth' });
    };

    if (isLoading) {
        return (
        <div className="flex items-center justify-center py-20">
            <Loader2 className="w-8 h-8 animate-spin text-[#8B7A9F]" />
        </div>
        );
    }

    return (
        <div>
            <div className="space-y-6 pb-8">
                <div className="mb-2 flex justify-between items-center">
                    <h1 className="text-2xl font-semibold text-[#4A4458]">Baby Dashboard</h1>
                    <button
                    onClick={() => navigate(createPageUrl('Profile'))}
                    className="p-2 hover:bg-white/50 rounded-full transition-colors"
                    >
                    <UserCircle className="w-8 h-8 text-[#8B7A9F]" />
                    </button>
                </div>
            </div>

        <QuickAddSection 
            onActivityAdded={handleActivityAdded} 
            editingActivity={editingActivity}
            onCancelEdit={() => setEditingActivity(null)}
        />
        <UpcomingTasks/>
        <HistorySection 
            activities={activities} 
            onEditActivity={handleEditActivity}
        />
        </div>
    );
    }