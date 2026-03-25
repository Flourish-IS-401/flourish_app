import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { base44 } from '@/api/base44Client';
import { listBabyActivities } from '@/api/babyActivityApi';
import { listBabyMoods } from '@/api/babyMoodApi';
import { format, isSameDay } from 'date-fns';
import { useNavigate, useLocation } from 'react-router-dom';
import DayDetailsDropdowns from '@/components/calendar/DayDetailsDropdowns';
import MonthView from '@/components/calendar/MonthView';

export default function Calendar() {
    const navigate = useNavigate();
    const location = useLocation();

    const activeTab = location.pathname === '/insights' ? 'insights' : 'calendar';

    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedDate, setSelectedDate] = useState(new Date());

    const { data: moodEntries = [] } = useQuery({
        queryKey: ['moodEntries'],
        queryFn: () => base44.entities.MoodEntry.list('-date', 100),
    });

    const { data: babyActivities = [] } = useQuery({
        queryKey: ['babyActivities'],
        queryFn: () => listBabyActivities({ sort: '-timestamp', limit: 200 }),
    });

    const { data: babyMoods = [] } = useQuery({
        queryKey: ['babyMoods'],
        queryFn: () => listBabyMoods({ sort: '-timestamp', limit: 200 }),
    });

    const { data: journalEntries = [] } = useQuery({
        queryKey: ['journalEntriesCalendar'],
        queryFn: () => base44.entities.JournalEntry.list('-created_date', 100),
    });

    const getMoodsForDate = (date) => {
        const dateStr = format(date, 'yyyy-MM-dd');
        return moodEntries.filter(m => m.date === dateStr);
    };

    const getActivitiesForDate = (date) => {
        return babyActivities.filter((a) => {
            const ts = a.timestamp ?? a.Timestamp;
            return ts && isSameDay(new Date(ts), date);
        });
    };

    const getBabyMoodsForDate = (date) => {
        return babyMoods.filter((m) => {
            const ts = m.timestamp ?? m.Timestamp;
            return ts && isSameDay(new Date(ts), date);
        });
    };

    const getJournalEntriesForDate = (date) => {
        return journalEntries.filter(j => isSameDay(new Date(j.created_date), date));
    };

    const selectedMoods = getMoodsForDate(selectedDate);
    const selectedActivities = getActivitiesForDate(selectedDate);
    const selectedBabyMoods = getBabyMoodsForDate(selectedDate);
    const selectedJournals = getJournalEntriesForDate(selectedDate);

    return (
        <div className="space-y-6 pb-8">
            <div className="flex gap-2 p-1 bg-[#E8E4F3]/50 rounded-2xl">
                <button
                    onClick={() => navigate('/calendar')}
                    className={`flex-1 py-2.5 rounded-xl text-sm font-medium transition-all ${
                        activeTab === 'calendar'
                            ? 'bg-white text-[#4A4458] shadow-sm'
                            : 'text-[#5A4B70]'
                    }`}
                >
                    Calendar
                </button>

                <button
                    onClick={() => navigate('/insights')}
                    className={`flex-1 py-2.5 rounded-xl text-sm font-medium transition-all ${
                        activeTab === 'insights'
                            ? 'bg-white text-[#4A4458] shadow-sm'
                            : 'text-[#5A4B70]'
                    }`}
                >
                    Insights
                </button>
            </div>

            <MonthView
                currentDate={currentDate}
                setCurrentDate={setCurrentDate}
                selectedDate={selectedDate}
                setSelectedDate={setSelectedDate}
                moodEntries={moodEntries}
            />

            <div className="bg-white rounded-3xl p-6 shadow-sm">
                <h3 className="font-semibold text-[#4A4458] mb-4">
                    {format(selectedDate, 'EEEE, MMMM d, yyyy')}
                </h3>

                <DayDetailsDropdowns
                    moodEntries={selectedMoods}
                    babyActivities={selectedActivities}
                    babyMoods={selectedBabyMoods}
                    journalEntries={selectedJournals}
                />
            </div>
        </div>
    );
}